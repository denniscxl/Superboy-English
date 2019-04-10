using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Main : MonoBehaviour
{

    [SerializeField]
    private RawImage _img;
    [SerializeField]
    private Button _nextBtn;
    [SerializeField]
    private Button _exitBtn;
    [SerializeField]
    private Text _time;
    public InputField _eTime;

    private int count;
    private string[] files;
    private string path = "";
    private float lastTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        _nextBtn.onClick.AddListener(OnNextBtn);
        _exitBtn.onClick.AddListener(OnExitBtn);

        path = string.Format("{0}/../Words/", Application.dataPath);
        files = Directory.GetFiles(path);
        count = files.Length;
        Next();
    }

    // Update is called once per frame
    void Update()
    {
        float _t = Time.time - lastTime;
        if (_t > int.Parse(_eTime.text))
        {
            lastTime = Time.time;
            Next();
        }
        _time.text = _t.ToString();
    }

    #region ButtonDelegate
    private void OnNextBtn()
    {
        //Debug.Log("next");
        Next();
    }

    private void OnExitBtn()
    {
        //Debug.Log("exit");
        Application.Quit();
    }
    #endregion

    #region Logic
    private void Next()
    {
        if (0 != count)
        {
            int rd = Random.Range(0, count);
            string fn = files[rd];
            //StartCoroutine(LoadImg(fn));
            LoadFromFile(fn);
        }
    }


    private IEnumerator LoadImg(string name)
    {
        string destPath = string.Format("file://{0}", name);
        using (UnityWebRequest www = new UnityWebRequest(destPath))
        {
            yield return www;
            if (www != null && string.IsNullOrEmpty(www.error) && www.isDone)
            {
                int width = 200;
                int height = 200;
                Texture2D texture = new Texture2D(width, height);
                texture.LoadImage(www.downloadHandler.data);
                _img.texture = texture;
            }
            else
            {
                Debug.LogError("LoadImg faile! " + www.error);
            }
        }
    }

    private void LoadFromFile(string fn)
    {
        string destPath = string.Format("{0}", fn);

        FileStream fileStream = new FileStream(destPath, FileMode.Open, FileAccess.Read);
        byte[] bytes = new byte[fileStream.Length];
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;

        int width = 300;
        int height = 300;
        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(bytes);
        _img.texture = texture;
    }
    #endregion
}
