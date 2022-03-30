using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR 
using UnityEditor;
#endif

namespace Maze.Mechanics {
    public class DecalPolygon {

        public List<Vector3> vertices = new List<Vector3>(9);

        public DecalPolygon(params Vector3[] vts) {
            vertices.AddRange(vts);
        }

        public static DecalPolygon ClipPolygon(DecalPolygon polygon, Plane plane) {
            bool[] positive = new bool[9];
            int positiveCount = 0;

            for (int i = 0; i < polygon.vertices.Count; i++) {
                positive[i] = !plane.GetSide(polygon.vertices[i]);
                if (positive[i]) positiveCount++;
            }

            if (positiveCount == 0) return null; // полностью за плоскостью
            if (positiveCount == polygon.vertices.Count) return polygon; // полностью перед плоскостью

            DecalPolygon tempPolygon = new DecalPolygon();

            for (int i = 0; i < polygon.vertices.Count; i++) {
                int next = i + 1;
                next %= polygon.vertices.Count;

                if (positive[i]) {
                    tempPolygon.vertices.Add(polygon.vertices[i]);
                }

                if (positive[i] != positive[next]) {
                    Vector3 v1 = polygon.vertices[next];
                    Vector3 v2 = polygon.vertices[i];

                    Vector3 v = LineCast(plane, v1, v2);
                    tempPolygon.vertices.Add(v);
                }
            }

            return tempPolygon;
        }

        private static Vector3 LineCast(Plane plane, Vector3 a, Vector3 b) {
            float dis;
            Ray ray = new Ray(a, b - a);
            plane.Raycast(ray, out dis);
            return ray.GetPoint(dis);
        }

    }

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Decal : MonoBehaviour {

        //public Sprite sprite;
        public List<Texture2D> texs = new List<Texture2D>();

        public float maxAngle = 90.0f;
        public float pushDistance = 0.009f;
        public LayerMask affectedLayers = -1;
        private GameObject[] affectedObjects;
        public float lifeTime = 0;
        float lifeTimeCur = 0;
        public Vector2 tiling = Vector2.one;
        public Vector2 offset = Vector2.zero;
        public Vector2 deltaOffset = Vector2.zero;
        public float offsetTime = 1;
        float curOffsetTime = 0;
        public bool randomOffset = false;
        public Vector2 startScale = Vector2.one;
        public Vector2 endScale = Vector2.one;
        //public float rot = 0;
        public float deltaRot = 0;
        public float deltaTex = 0;
        public bool randomTex = false;
        float texTimer = 0;
        public int curTex = -1;
        Vector2 curScale = Vector2.one;
        Vector3 realScale = Vector3.one;
        public bool continuous = true;

        public AudioSource sfx = null;
        bool inited = false;

        bool enableSound = false;

        public List<Color> colors = new List<Color>();
        public List<float> alpha = new List<float>();

        float sqr2 = Mathf.Sqrt(2);

        public bool changed = true;

        MeshFilter filter;
        MeshRenderer renderer;
        Material material;

        string colorPropertyName = null;

        void OnDrawGizmosSelected() {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
        void Start() {
            lifeTimeCur = lifeTime;
            curOffsetTime = offsetTime;
            texTimer = deltaTex;
            realScale = transform.localScale;
            filter = GetComponent<MeshFilter>();
            renderer = GetComponent<MeshRenderer>();
            if (colorPropertyName == null && renderer.material.HasProperty("_Color")) colorPropertyName = "_Color";
            if (colorPropertyName == null && renderer.material.HasProperty("_TintColor")) colorPropertyName = "_TintColor";
            if (colorPropertyName == null && renderer.material.HasProperty("_Tint")) colorPropertyName = "_Tint";
            if (colorPropertyName != null) {
                Color color = renderer.material.GetColor(colorPropertyName);
                if (colors.Count > 0) {
                    color = colors[0];
                }
                if (alpha.Count > 0) {
                    color.a = alpha[0];
                }
                renderer.material.SetColor(colorPropertyName, color);
            }
            //if (texs.Count > 0) {
                BuildDecal();
            //}
        }

        void Update() {
            if (!inited) {
                if (enableSound) {
                    sfx.Play();
                }
                inited = true;
            }
            if (lifeTime > 0) {
                if (lifeTimeCur > 0) {
                    lifeTimeCur -= Time.deltaTime;
                    if (texs.Count > 0) {
                        if (deltaRot != 0) {
                            transform.Rotate(Vector3.forward, deltaRot * Time.deltaTime, Space.Self);
                            changed = true;
                        }
                        if (startScale != endScale) {
                            curScale = Vector2.Lerp(startScale, endScale, 1 - (lifeTimeCur / lifeTime));
                            transform.localScale = new Vector3(realScale.x * curScale.x, realScale.y * curScale.y, realScale.z);
                            changed = true;
                        }
                        if (deltaOffset != Vector2.zero) {
                            if (curOffsetTime > 0) {
                                curOffsetTime -= Time.deltaTime;
                            } else {
                                if (randomOffset) {
                                    offset += deltaOffset * Random.Range(1, 17);
                                } else {
                                    offset += deltaOffset;
                                }
                                renderer.material.mainTextureOffset = offset;
                                curOffsetTime = offsetTime;
                            }
                        }
                        int tc = texs.Count;
                        if (tc > 1) {
                            if (texTimer > 0) {
                                texTimer -= Time.deltaTime;
                            } else {
                                if (randomTex) {
                                    curTex = Random.Range(0, tc - 1);
                                } else {
                                    curTex++;
                                    while (curTex > tc - 1) {
                                        curTex -= tc;
                                    }
                                }
                                renderer.material.mainTexture = texs[curTex];
                                if (deltaTex > 0) {
                                    while (texTimer <= 0) {
                                        texTimer += deltaTex;
                                    }
                                }
                            }
                        }
                        int cc = colors.Count;
                        int ac = alpha.Count;
                        if (cc > 1 || ac > 1) {
                            Color lerped = renderer.material.GetColor(colorPropertyName);
                            if (cc > 1) {
                                float qwer = (lifeTime - lifeTimeCur) / lifeTime * (colors.Count - 1);
                                int asasdf = Mathf.FloorToInt(qwer);
                                lerped = colors[asasdf];
                                if (asasdf < colors.Count - 1) {
                                    lerped = Color.Lerp(colors[asasdf], colors[asasdf + 1], qwer - asasdf);
                                }
                            }
                            if (ac > 1) {
                                float zxcv = (lifeTime - lifeTimeCur) / lifeTime * (alpha.Count - 1);
                                int bvcx = Mathf.FloorToInt(zxcv);
                                float alper = alpha[bvcx];
                                if (bvcx < alpha.Count - 1) {
                                    alper = Mathf.Lerp(alpha[bvcx], alpha[bvcx + 1], zxcv - bvcx);
                                }
                                lerped.a = alper;
                            }
                            renderer.material.SetColor(colorPropertyName, lerped);
                        }
                        if (transform.hasChanged) changed = true;
                        if (changed) BuildDecal();

                    }
                } else {
                    lifeTimeCur += lifeTime;
                    if (!continuous) {
                        Destroy(transform.root.gameObject);
                    }
                }
            }

        }

        public Bounds GetBounds() {
            Vector3 size = transform.lossyScale;
            Vector3 min = -size / 2f;
            //min = new Vector3 (min.x * sqr2, min.y * sqr2, min.z);
            Vector3 max = size / 2f;
            //max = new Vector3 (max.x * sqr2, max.y * sqr2, max.z);

            Vector3[] vts = new Vector3[] {
            new Vector3(min.x, min.y, min.z),
            new Vector3(max.x, min.y, min.z),
            new Vector3(min.x, max.y, min.z),
            new Vector3(max.x, max.y, min.z),

            new Vector3(min.x, min.y, max.z),
            new Vector3(max.x, min.y, max.z),
            new Vector3(min.x, max.y, max.z),
            new Vector3(max.x, max.y, max.z),
        };

            for (int i = 0; i < 8; i++) {
                vts[i] = transform.TransformDirection(vts[i]);
            }

            min = max = vts[0];
            foreach (Vector3 v in vts) {
                min = Vector3.Min(min, v);
                max = Vector3.Max(max, v);
            }

            return new Bounds(transform.position, max - min);
        }


        public void BuildDecal() {
            changed = false;
            if(texs.Count > 0) renderer.material.mainTexture = texs[curTex];
             filter = GetComponent<MeshFilter>();
            if (filter == null) filter = gameObject.AddComponent<MeshFilter>();
            if (renderer == null) gameObject.AddComponent<MeshRenderer>();
            /*decal.GetComponent<Renderer>().material = decal.material;

            if(decal.material == null || decal.sprite == null) {
                filter.mesh = null;
                return;
            }*/

            affectedObjects = GetAffectedObjects(GetBounds(), affectedLayers);
            foreach (GameObject go in affectedObjects) {
                BuildDecalForObject(go);
            }
            Push(pushDistance);

            Mesh mesh = CreateMesh();
            if (mesh != null) {
                mesh.name = "DecalMesh";
                filter.mesh = mesh;
            }
            //renderer.materials[0].color = Color.white;
        }
        private static GameObject[] GetAffectedObjects(Bounds bounds, LayerMask affectedLayers) {
            MeshRenderer[] renderers = (MeshRenderer[])GameObject.FindObjectsOfType<MeshRenderer>();
            List<GameObject> objects = new List<GameObject>();
            foreach (Renderer r in renderers) {
                if (!r.enabled) continue;
                if (!IsLayerContains(affectedLayers, r.gameObject.layer)) continue;
                if (r.GetComponent<Decal>() != null) continue;

                if (bounds.Intersects(r.bounds)) {
                    objects.Add(r.gameObject);
                }
            }
            return objects.ToArray();
        }

        private static bool IsLayerContains(LayerMask mask, int layer) {
            return (mask.value & 1 << layer) != 0;
        }


        private List<Vector3> bufVertices = new List<Vector3>();
        private List<Vector3> bufNormals = new List<Vector3>();
        private List<Vector2> bufTexCoords = new List<Vector2>();
        private List<int> bufIndices = new List<int>();
        int startVertexCount;


        public void BuildDecalForObject(GameObject affectedObject) {
            bufVertices.Clear();
            bufNormals.Clear();
            bufTexCoords.Clear();
            bufIndices.Clear();
            Mesh affectedMesh = affectedObject.GetComponent<MeshFilter>().sharedMesh;
            if (affectedMesh == null) return;

            Plane right = new Plane(Vector3.right, Vector3.right / 2f);
            Plane left = new Plane(-Vector3.right, -Vector3.right / 2f);

            Plane top = new Plane(Vector3.up, Vector3.up / 2f);
            Plane bottom = new Plane(-Vector3.up, -Vector3.up / 2f);

            Plane front = new Plane(Vector3.forward, Vector3.forward / 2f);
            Plane back = new Plane(-Vector3.forward, -Vector3.forward / 2f);

            Vector3[] vertices = affectedMesh.vertices;
            int[] triangles = affectedMesh.triangles;
            startVertexCount = bufVertices.Count;

            Matrix4x4 matrix = transform.worldToLocalMatrix * affectedObject.transform.localToWorldMatrix;

            for (int i = 0; i < triangles.Length; i += 3) {
                int i1 = triangles[i];
                int i2 = triangles[i + 1];
                int i3 = triangles[i + 2];

                Vector3 v1 = matrix.MultiplyPoint(vertices[i1]);
                Vector3 v2 = matrix.MultiplyPoint(vertices[i2]);
                Vector3 v3 = matrix.MultiplyPoint(vertices[i3]);

                Vector3 side1 = v2 - v1;
                Vector3 side2 = v3 - v1;
                Vector3 normal = Vector3.Cross(side1, side2).normalized;

                if (Vector3.Angle(-Vector3.forward, normal) >= maxAngle) continue;


                DecalPolygon poly = new DecalPolygon(v1, v2, v3);

                poly = DecalPolygon.ClipPolygon(poly, right);
                if (poly == null) continue;
                poly = DecalPolygon.ClipPolygon(poly, left);
                if (poly == null) continue;

                poly = DecalPolygon.ClipPolygon(poly, top);
                if (poly == null) continue;
                poly = DecalPolygon.ClipPolygon(poly, bottom);
                if (poly == null) continue;

                poly = DecalPolygon.ClipPolygon(poly, front);
                if (poly == null) continue;
                poly = DecalPolygon.ClipPolygon(poly, back);
                if (poly == null) continue;

                AddPolygon(poly, normal);
            }

            GenerateTexCoords(startVertexCount);
        }

        private void AddPolygon(DecalPolygon poly, Vector3 normal) {
            int ind1 = AddVertex(poly.vertices[0], normal);
            for (int i = 1; i < poly.vertices.Count - 1; i++) {
                int ind2 = AddVertex(poly.vertices[i], normal);
                int ind3 = AddVertex(poly.vertices[i + 1], normal);

                bufIndices.Add(ind1);
                bufIndices.Add(ind2);
                bufIndices.Add(ind3);
            }
        }

        private int AddVertex(Vector3 vertex, Vector3 normal) {
            int index = FindVertex(vertex);
            if (index == -1) {
                bufVertices.Add(vertex);
                bufNormals.Add(normal);
                index = bufVertices.Count - 1;
            } else {
                Vector3 t = bufNormals[index] + normal;
                bufNormals[index] = t.normalized;
            }
            return (int)index;
        }

        private int FindVertex(Vector3 vertex) {
            for (int i = 0; i < bufVertices.Count; i++) {
                if (Vector3.Distance(bufVertices[i], vertex) < 0.01f) {
                    return i;
                }
            }
            return -1;
        }

        private void GenerateTexCoords(int start) {

            /*Rect rect = sprite.rect;
            rect.x /= sprite.texture.width;
            rect.y /= sprite.texture.height;
            rect.width /= sprite.texture.width;
            rect.height /= sprite.texture.height;*/

            Vector2 bound = offset + tiling;
            for (int i = start; i < bufVertices.Count; i++) {
                Vector3 vertex = bufVertices[i];
                Vector2 uv = new Vector2(vertex.x + 0.5f, vertex.y + 0.5f);
                uv.x = Mathf.Lerp(offset.x, bound.x, uv.x);
                uv.y = Mathf.Lerp(offset.y, bound.y, uv.y);

                bufTexCoords.Add(uv);
            }
        }

        public void Push(float distance) {
            for (int i = 0; i < bufVertices.Count; i++) {
                Vector3 normal = bufNormals[i];
                bufVertices[i] += normal * distance;
            }
        }


        public Mesh CreateMesh() {
            if (bufIndices.Count == 0) {
                return null;
            }
            Mesh mesh = new Mesh();

            mesh.vertices = bufVertices.ToArray();
            mesh.normals = bufNormals.ToArray();
            mesh.uv = bufTexCoords.ToArray();
            mesh.uv2 = bufTexCoords.ToArray();
            mesh.triangles = bufIndices.ToArray();

            bufVertices.Clear();
            bufNormals.Clear();
            bufTexCoords.Clear();
            bufIndices.Clear();

            return mesh;
        }
    }
}