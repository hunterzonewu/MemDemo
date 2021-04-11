using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Text;

public class GraphicMemTest : MonoBehaviour
{
    public List<int[]> arrList = new List<int[]>();
    public List<GameObject> avatarList = new List<GameObject>();
    public Transform panel = null;
    public string winShader = "";
    public string mobileShader = "";
    public string scene = "";
    public string depAB = "";
    public string ab = "";
    public string avatar = "";
    public AssetBundle shaderAB = null;
    public AssetBundle sceneAB = null;
    public AssetBundle depABAB = null;
    public AssetBundle abAB = null;

    public string tex_prefab = "";
    public string mesh_prefab = "";
    public string mesh_noskin_prefab = "";
    public string particle_prefab = "";
    public string go_prefab = "";
    public string font_prefab = "";
    public string anim = "";
    public GameObject avatarTemplate = null;
    public Animation animComp = null;
    public int rtNum = 0;
    public List<RenderTexture> rtList = new List<RenderTexture>();
    public ShaderIndexData _shaderIdxData = null;
    public int shaderNum = 0;
    public StringBuilder m_sb = new StringBuilder();
    public int sbUseCount = 0;
    public List<GameObject> particleAssetList = null;
    public List<GameObject> particleInsList = null;
    public bool bShowParticleIns = true;

    void onSceneLoad(Scene scene, LoadSceneMode lsm)
    {
        depABAB = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, depAB));
        abAB = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, ab));
        GameObject abGo = abAB.LoadAsset<GameObject>(Path.GetFileNameWithoutExtension(ab));
        GameObject.Instantiate(abGo);
    }

    private GameObject createGo(string abPath)
    {
        loadShader();
        AssetBundle avatarAB = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, abPath));
        GameObject go = avatarAB.LoadAsset<GameObject>(Path.GetFileNameWithoutExtension(abPath));
        GameObject goIns = GameObject.Instantiate(go);
        avatarAB.Unload(false);
        return goIns;
    }

    private void loadShader()
    {
        if (null != shaderAB)
            return;
        string shaderPath = Path.Combine(Application.streamingAssetsPath, mobileShader);
#if UNITY_EDITOR
        shaderPath = Path.Combine(Application.streamingAssetsPath, winShader);
#endif
        shaderAB = AssetBundle.LoadFromFile(shaderPath);
        _shaderIdxData = shaderAB.LoadAsset<ShaderIndexData>("assets/resources/shaders/svc/shader_idx.asset");
    }

    private void OnGUI_sb()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("测试StringBuilder", GUILayout.MaxHeight(Screen.height / 10), GUILayout.MaxWidth(Screen.width / 4)))
        {
            var sb = m_sb;
            for (int i = 0; i < 1000; ++i)
            {
                sb.AppendLine("objectsBackMap data");
                sb.AppendLine($"null object {++sbUseCount}");
            }
            //Debug.Log(sb.ToString());
        }
        GUILayout.EndHorizontal();
    }

    private void OnGUI_pss()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("测试shaderlab", GUILayout.MaxHeight(Screen.height / 10), GUILayout.MaxWidth(Screen.width / 4)))
        {
            loadShader();
            shaderNum += 20;
            for (int i=0; i<shaderNum; ++i)
            {
                shaderAB.LoadAsset<Shader>(_shaderIdxData.shaderIdxList[i].abName);
            }
        }
        if (GUILayout.Button("测试font", GUILayout.MaxHeight(Screen.height / 10), GUILayout.MaxWidth(Screen.width / 4)))
        {
            for (int i = 0; i < 20; ++i)
            {
                createGo(font_prefab);
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("测试贴图", GUILayout.MaxHeight(Screen.height / 10), GUILayout.MaxWidth(Screen.width / 4)))
        {
            for (int i=0; i<20; ++i)
            {
                createGo(tex_prefab);
            }
        }
        if (GUILayout.Button("测试skin mesh", GUILayout.MaxHeight(Screen.height / 10), GUILayout.MaxWidth(Screen.width / 4)))
        {
            for (int i = 0; i < 20; ++i)
            {
                GameObject go = createGo(mesh_prefab);
            }
        }
        if (GUILayout.Button("测试no-skin mesh", GUILayout.MaxHeight(Screen.height / 10), GUILayout.MaxWidth(Screen.width / 4)))
        {
            for (int i = 0; i < 20; ++i)
            {
                GameObject go = createGo(mesh_noskin_prefab);
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("测试动作", GUILayout.MaxHeight(Screen.height / 10), GUILayout.MaxWidth(Screen.width / 4)))
        {
            if (null == avatarTemplate)
            {
                avatarTemplate = createGo(mesh_prefab);
                animComp = avatarTemplate.GetComponentInChildren<Animation>();
            }
            string name = "";
            for (int i=0; i<20; ++i)
            {
                AssetBundle avatarAB = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, anim));
                AnimationClip clip = avatarAB.LoadAsset<AnimationClip>(Path.GetFileNameWithoutExtension(anim));
                name = clip.name+animComp.GetClipCount();
                animComp.AddClip(clip, name);
                avatarAB.Unload(false);
            }
            animComp.Play(name);
        }

        if (GUILayout.Button("测试粒子（实例）", GUILayout.MaxHeight(Screen.height / 10), GUILayout.MaxWidth(Screen.width / 4)))
        {
            if (null == particleInsList)
                particleInsList = new List<GameObject>(1024 * 4);
            for (int i = 0; i < 20; ++i)
            {
                GameObject go = createGo(particle_prefab);
                particleInsList.Add(go);
            }
        }
        if (GUILayout.Button("测试粒子（资源）", GUILayout.MaxHeight(Screen.height / 10), GUILayout.MaxWidth(Screen.width / 4)))
        {
            if (null == particleAssetList)
                particleAssetList = new List<GameObject>(1024*4);
            for (int i = 0; i < 100; ++i)
            {
                loadShader();
                AssetBundle avatarAB = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, particle_prefab));
                GameObject go = avatarAB.LoadAsset<GameObject>(Path.GetFileNameWithoutExtension(particle_prefab));
                particleAssetList.Add(go);
                avatarAB.Unload(false);
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("测试RT", GUILayout.MaxHeight(Screen.height / 10), GUILayout.MaxWidth(Screen.width / 4)))
        {
            rtNum += 10;
        }
        if (GUILayout.Button("测试Go数量", GUILayout.MaxHeight(Screen.height / 10), GUILayout.MaxWidth(Screen.width / 4)))
        {
            for (int i = 0; i < 20; ++i)
            {
                createGo(go_prefab);
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("显隐粒子系统", GUILayout.MaxHeight(Screen.height / 10), GUILayout.MaxWidth(Screen.width / 4)))
        {
            bShowParticleIns = !bShowParticleIns;
            for (int i = 0; i < particleInsList.Count; ++i)
            {
                if (null == particleInsList[i])
                    continue;
                particleInsList[i].SetActive(bShowParticleIns);
            }
        }
        if (GUILayout.Button("删除粒子系统", GUILayout.MaxHeight(Screen.height / 10), GUILayout.MaxWidth(Screen.width / 4)))
        {
            bShowParticleIns = !bShowParticleIns;
            for (int i = 0; i < particleInsList.Count; ++i)
            {
                if (null == particleInsList[i])
                    continue;
                Destroy(particleInsList[i]);
            }
            particleInsList.Clear();
        }
        if (GUILayout.Button("GC", GUILayout.MaxHeight(Screen.height / 10), GUILayout.MaxWidth(Screen.width / 4)))
        {
            Resources.UnloadUnusedAssets();
        }
        GUILayout.EndHorizontal();
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnGUI()
    {
        if (GUILayout.Button("加载场景", GUILayout.MaxHeight(Screen.height / 10), GUILayout.MaxWidth(Screen.width / 4)))
        {
            string shaderPath = Path.Combine(Application.streamingAssetsPath, mobileShader);
#if UNITY_EDITOR
            shaderPath = Path.Combine(Application.streamingAssetsPath, winShader);
#endif
            shaderAB = AssetBundle.LoadFromFile(shaderPath);
            /*
            sceneAB = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, scene));
            SceneManager.sceneLoaded += onSceneLoad;
            SceneManager.LoadScene(Path.GetFileNameWithoutExtension(scene));
            */
        }
        if (GUILayout.Button("添加角色", GUILayout.MaxHeight(Screen.height / 10), GUILayout.MaxWidth(Screen.width / 4)))
        {
            for (int i = 0; i < 20; ++i)
            {
                AssetBundle avatarAB = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, avatar));
                GameObject go = avatarAB.LoadAsset<GameObject>(Path.GetFileNameWithoutExtension(avatar));
                GameObject goIns = GameObject.Instantiate(go);
                Vector3 pos = goIns.transform.position;
                int oo = avatarList.Count % 20;
                int aa = oo % 2;
                int bb = (oo + aa) / 2 * (aa * 2 - 1);

                int cc = avatarList.Count / 20;
                int dd = cc % 2;
                int ee = (cc + dd) / 2 * (dd * 2 - 1);
                Debug.Log(ee);
                goIns.transform.position = new Vector3(pos.x + (float)1.5 * bb, pos.y, pos.z+ee);
                avatarList.Add(goIns);
                avatarAB.Unload(false);
                string clipName = (avatarList.Count%25).ToString();
                AnimationClip clip = Resources.Load<AnimationClip>(clipName);
                Animation anim = goIns.GetComponent<Animation>();
                anim.AddClip(clip, clipName);
                anim.Play(clipName);
            }
        }
        if (GUILayout.Button("删除角色", GUILayout.MaxHeight(Screen.height / 10), GUILayout.MaxWidth(Screen.width / 4)))
        {
            for (int i = 0; i < 15; ++i)
            {
                if (avatarList.Count == 0)
                    break;
                GameObject.Destroy(avatarList[avatarList.Count - 1]);
                avatarList.RemoveAt(avatarList.Count - 1);
            }
        }
        if (GUILayout.Button("ADD Img", GUILayout.MaxHeight(Screen.height / 10), GUILayout.MaxWidth(Screen.width / 4)))
        { }
        if (GUILayout.Button("DEL Img", GUILayout.MaxHeight(Screen.height / 10), GUILayout.MaxWidth(Screen.width / 4)))
        { }
        if (GUILayout.Button("NEW Mem", GUILayout.MaxHeight(Screen.height / 10), GUILayout.MaxWidth(Screen.width / 4)))
        {
            for (ulong i = 0; i < 20; ++i)
            {
                ulong len = 512 * 1024*(i%3+1);
                int[] intArr = new int[len];
                for (ulong j = 0; j < len; ++j)
                {
                    intArr[j] = (int)j / 1024;
                }
                arrList.Add(intArr);
            }
        }
        if (GUILayout.Button("Free Mem", GUILayout.MaxHeight(Screen.height / 10), GUILayout.MaxWidth(Screen.width / 4)))
        {
            for (ulong i = 0; i < 8; ++i)
            {
                if (arrList.Count > 0)
                    arrList.RemoveAt(arrList.Count-arrList.Count%8-1);
            }
            System.GC.Collect();
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture src = source;
        for (int i=0; i<rtNum; ++i)
        {
            RenderTexture rt = RenderTexture.GetTemporary(1024, 1024, 0);
            rtList.Add(rt);
            Graphics.Blit(src, rt);
            src = rt;
        }
        Graphics.Blit(src, destination);

        for (int i=0; i<rtList.Count; ++i)
        {
            RenderTexture.ReleaseTemporary(rtList[i]);
        }
    }
}
