using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;


struct Texture2DInfo
{
    public Sprite SpriteRef;
    public FileInfo FI;
}

public class Main : MonoBehaviour
{
    List<Texture2DInfo> TexPool;
    int TexPoolSize = 15;

    [SerializeField]
    Image PicPreviewImg;

    Texture2D targetTex;
    // Use this for initialization

    //Import the following.
    [DllImport("user32", EntryPoint = "SetWindowTextW", CharSet = CharSet.Unicode)]
    public static extern bool SetWindowTextW(System.IntPtr hwnd, System.String lpString);
    [DllImport("user32", EntryPoint = "FindWindow")]
    public static extern System.IntPtr FindWindow(System.String className, System.String windowName);

    IntPtr windowPtr;
    RectTransform TransRef;
    void Start()
    {
        string[] CommandLineArgs = /*{ "", @"C:\Users\ZipZapWxh\Pictures\ff\47830019_p0.jpg" };*/ Environment.GetCommandLineArgs();
        if(CommandLineArgs.Length < 2)
        {
            Application.Quit();
        }
        TexPool = new List<Texture2DInfo>();
        string filename = CommandLineArgs[1];
        PicPreviewImg.sprite = HotLoadPicture(filename);
        targetTex = PicPreviewImg.sprite.texture;
        Texture2DInfo tmpTI = new Texture2DInfo();
        tmpTI.SpriteRef = PicPreviewImg.sprite;
        tmpTI.FI = new FileInfo(filename);
        TexPool.Add(tmpTI);
        ++ImgIndex; 
        //  = Sprite.Create(targetTex, new Rect(0, 0, targetTex.width, targetTex.height), new Vector2(0.5f, 0.5f));
        //  Sprite.Create(targetTex)
        ShowPreviewPic(targetTex);
        windowPtr = FindWindow(null, "PhotoViewer");
        SetWindowTextW(windowPtr, filename);
        TransRef = this.transform as RectTransform;
        MousePos = Input.mousePosition;
        ImgOriginPos = TransRef.position;
    }

    // Update is called once per frame
    Vector3 MousePos;
    Vector3 ImgOriginPos;
    void Update()
    {
        ShowPreviewPic(targetTex);

        var MousePosTmp = Input.mousePosition;
        var deltaMouse = MousePosTmp - MousePos;
        MousePos = MousePosTmp;
        if (Input.GetMouseButton(0))
        {

            TransRef.position = TransRef.position + new Vector3(deltaMouse.x, deltaMouse.y, 0);

        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            //Vector3 MouseCenterPos = Input.mousePosition;//- new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            //TransRef.position = Vector3.Lerp(TransRef.position, MouseCenterPos, 0.2f);
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {


                TransRef.localScale = TransRef.localScale * 1.2f;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {


                TransRef.localScale = TransRef.localScale / 1.2f;
            }
        }
    }

    Vector2 GetWindowSize()
    {

        return new Vector2(Screen.width, Screen.height);
    }
    public void ShowPreviewPic(Texture2D tex)
    {
        Vector2 PreviewImgBound = GetWindowSize();
        //PicPreviewImg.transform.parent.gameObject.SetActive(true);
        float xdelta = (tex.width - PreviewImgBound.x) * tex.width;
        float ydelta = (tex.height - PreviewImgBound.y) * tex.height;
        // PicPreviewImg.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        if (xdelta < 0 && ydelta < 0)
        {
            PicPreviewImg.rectTransform.sizeDelta = new Vector2(tex.width, tex.height);
        }
        if (xdelta < 0 && ydelta > 0)
        {
            float dvid = tex.height / PreviewImgBound.y;
            PicPreviewImg.rectTransform.sizeDelta = new Vector2(tex.width / dvid, tex.height / dvid);
        }
        if (xdelta > 0 && ydelta < 0)
        {
            float dvid = tex.width / PreviewImgBound.x;
            PicPreviewImg.rectTransform.sizeDelta = new Vector2(tex.width / dvid, tex.height / dvid);
        }
        if (xdelta > 0 && ydelta > 0)
        {
            float dvid = 0;
            if (xdelta > ydelta)
            {
                dvid = tex.width / PreviewImgBound.x;
                if (tex.height / dvid > PreviewImgBound.y)
                {
                    dvid = tex.height / PreviewImgBound.y;
                }
            }
            else if (xdelta <= ydelta)
            {
                dvid = tex.height / PreviewImgBound.y;
                if (tex.width / dvid > PreviewImgBound.x)
                {
                    dvid = tex.width / PreviewImgBound.x;

                }
            }


            PicPreviewImg.rectTransform.sizeDelta = new Vector2(tex.width / dvid + 0.05f, tex.height / dvid + 0.05f);
        }


    }
    DirectoryInfo dirinfo;
    int currentNum;
    public Sprite HotLoadPicture(string path)
    {

        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        FileInfo fileinfo = new FileInfo(path);
        dirinfo = fileinfo.Directory;
        FileInfo[] files = dirinfo.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].FullName.CompareTo(fileinfo.FullName) == 0)
            {
                currentNum = i;
                break;
            }
        }


        fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度的缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //释放文件读取liu
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;

        //创建Texture
        int width = 300;
        int height = 372;
        Texture2D texture2D = new Texture2D(width, height);
        texture2D.LoadImage(bytes);

        var tmpsprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
        
        return tmpsprite;
    }

    public void ResetImgTrans()
    {
        TransRef.position = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        TransRef.localScale = new Vector3(1, 1, 1);
        TransRef.rotation = Quaternion.identity;
    }

    public void RotateImg90()
    {
        TransRef.Rotate(new Vector3(0, 0, 90));
    }

    int ImgIndex = 0;
    public void leftRightButtonClick(int lr)
    {
        var files = dirinfo.GetFiles();
        if (lr == 1 && currentNum > 0)
        {
            int tmpCurrentNum = currentNum;
            string extension = "";
            bool extensionRe;
            do
            {
                currentNum--;
                extension = files[currentNum].Extension;
                extensionRe = extension != ".png" && extension != ".jpg" && extension != ".jpeg" && extension != ".tga" && extension != ".bmp";
            } while (extensionRe && currentNum != 0);

            if (currentNum == 0 && extensionRe)
            {
                currentNum = tmpCurrentNum;
                return;
            }
           
            if (ImgIndex >= 0)
            {
                if (TexPool[ImgIndex].FI.FullName.CompareTo(files[currentNum].FullName) == 0)
                {
                    PicPreviewImg.sprite = TexPool[ImgIndex].SpriteRef;
                    targetTex = PicPreviewImg.sprite.texture;
                    SetWindowTextW(windowPtr, files[currentNum].Name);
                    ShowPreviewPic(targetTex);
                }
                else if (TexPool[ImgIndex].FI.FullName.CompareTo(files[currentNum].FullName) != 0)
                {
                    PicPreviewImg.sprite = HotLoadPicture(files[currentNum].FullName);
                    targetTex = PicPreviewImg.sprite.texture;
                    var tmpTI = new Texture2DInfo();
                    tmpTI.SpriteRef = PicPreviewImg.sprite;
                    tmpTI.FI = files[currentNum];
                    TexPool.Insert(ImgIndex, tmpTI);
                    
                    SetWindowTextW(windowPtr, files[currentNum].Name);
                    ShowPreviewPic(targetTex);
                }
                --ImgIndex;
                return;
            }
          // else if (TexPool.Count > 0)
          // {
          //     ++ImgIndex;
          // }
          //
            PicPreviewImg.sprite = HotLoadPicture(files[currentNum].FullName);
            targetTex = PicPreviewImg.sprite.texture;
            var tmpTI1 = new Texture2DInfo();
            tmpTI1.SpriteRef = PicPreviewImg.sprite;
            tmpTI1.FI = files[currentNum];
            TexPool.Insert(ImgIndex, tmpTI1);
            ImgIndex++;
            //  Sprite.Create(targetTex)
            SetWindowTextW(windowPtr, files[currentNum].Name);
            ShowPreviewPic(targetTex);

        }
        if (lr == 2 && currentNum < files.Length)
        {
            int tmpCurrentNum = currentNum;
            string extension = "";
            bool extensionRe;
            do
            {
                currentNum++;
                extension = files[currentNum].Extension;
                extensionRe = extension != ".png" && extension != ".jpg" && extension != ".jpeg" && extension != ".tga" && extension != ".bmp";
            } while (extensionRe && currentNum < files.Length);
            if (currentNum >= files.Length && extensionRe)
            {
                currentNum = tmpCurrentNum;
                return;
            }
            
            if (ImgIndex < TexPool.Count)
            {
                if (TexPool[ImgIndex].FI.FullName.CompareTo(files[currentNum].FullName) == 0)
                {
                    PicPreviewImg.sprite = TexPool[ImgIndex].SpriteRef;
                    targetTex = PicPreviewImg.sprite.texture;
                    SetWindowTextW(windowPtr, files[currentNum].Name);
                    ShowPreviewPic(targetTex);
                }
                else if (TexPool[ImgIndex].FI.FullName.CompareTo(files[currentNum].FullName) != 0)
                {
                    PicPreviewImg.sprite = HotLoadPicture(files[currentNum].FullName);
                    targetTex = PicPreviewImg.sprite.texture;
                    var tmpTI = new Texture2DInfo();
                    tmpTI.SpriteRef = PicPreviewImg.sprite;
                    tmpTI.FI = files[currentNum];
                    TexPool.Add(tmpTI);
                    
                    SetWindowTextW(windowPtr, files[currentNum].Name);
                    ShowPreviewPic(targetTex);
                }
                ++ImgIndex;
                return;
            }
           // else if (TexPool.Count > 1)
           //     --ImgIndex;

            PicPreviewImg.sprite = HotLoadPicture(files[currentNum].FullName);
            targetTex = PicPreviewImg.sprite.texture;
            var tmpTI1 = new Texture2DInfo();
            tmpTI1.SpriteRef = PicPreviewImg.sprite;
            tmpTI1.FI = files[currentNum];
            TexPool.Add(tmpTI1);
            ImgIndex--;
            //targetTex = HotLoadPicture(files[currentNum].FullName);
            //PicPreviewImg.sprite = Sprite.Create(targetTex, new Rect(0, 0, targetTex.width, targetTex.height), new Vector2(0.5f, 0.5f));
            //  Sprite.Create(targetTex)
            SetWindowTextW(windowPtr, files[currentNum].Name);
            ShowPreviewPic(targetTex);
        }
       
    }
}
