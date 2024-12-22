/*
LevelMenu
Version 1.2

builds/rebuilds the LevelPages
deletes PlayerPrefs data for levels
manages the state of the scrollbar (Idle, Scrolling, Recoil)


*/


#if UNITY_EDITOR
    using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ScrollRect))]
[ExecuteInEditMode]
public class LevelMenu : MonoBehaviour , IPointerUpHandler , IPointerDownHandler , IDragHandler , IScrollHandler
{

#region Variables


    
    /// <summary>
    /// Deletes and Rebuild the LevelPages
    /// </summary>
    public bool rebuildLevelPages = false;

    /// <summary>
    /// Deletes and Rebuild the LevelPages after each update
    /// </summary>
    public bool autoRebuildLevelPages = false;

    /// <summary>
    /// The delete level data.
    /// </summary>
    public bool deleteLevelData = false;

    [Header("Level Page")]

    /// <summary>
    /// The level page constraint. (Flexible, Fixed Column Count, Fixed Row Count)
    /// </summary>
    public GridLayoutGroup.Constraint levelPageConstraint;

    /// <summary>
    /// The constraint count, for columns or rows (if needed)
    /// </summary>
    public int constraintCount = 3;

    /// <summary>
    /// Number of LevelButtons per each Page
    /// </summary>
    public int levelsPerPage = 12;

    /// <summary>
    /// the current selected LevelPage (Updates when the scrollBar states gets reset to Idle)
    /// </summary>
    public float selectedLevelPage;

    /// <summary>
    /// the current selected LevelPage as a float
    /// </summary>
    public float selectedLevelPageFloat;

    /// <summary>
    /// _SLP/SLP are used to set and update the selectedLevelPage/selectedLevelPageFloat
    /// </summary>
    private int _SLP;
    private int SLP
    {
        get {return _SLP;}

        set {
                _SLP = value;
                UpdateSLP(value + 1);
                PlayerPrefs.SetFloat("selectedLevelPage",selectedLevelPage);
            }
    }


    [Header("Level Button")]

    /// <summary>
    /// the Prefab that would be the template for all LevelButtons
    /// </summary>
    public GameObject levelButton;

    /// <summary>
    /// The empty level button used to fill in incomplete pages
    /// </summary>
    private GameObject emptyLevelButton;

    /// <summary>
    /// The size of the LevelButtons.
    /// </summary>
    public Vector2 buttonSize =  new Vector2(100f,100f);

    /// <summary>
    /// The spacing of the LevelButtons
    /// </summary>
    public Vector2 spacing =  new Vector2(100f,100f);


    [Header("Scroll Settings")]
    /// <summary>
    /// The direction you wish to scroll 
    /// </summary>
    public directions direction;

    /// <summary>
    /// possible scroll directions...more might be added at a later time
    /// </summary>
    public enum directions
    {
        Horizontal,
        Vertical_B2T, //Bottom to Top
        Vertical_T2B, //Top to Bottom
    }

    /// <summary>
    /// possible states for the scrollBar
    /// </summary>
    public enum states
    {
        IDLE,SCROLLING,RECOIL,AUTOSCROLLING
    }

    /// <summary>
    /// the current state of the scrollBar
    /// </summary>
    public states currentState;

    [Range(0.1f, 10.0f)]
    /// <summary>
    /// Speed of the scrollBar while Recoiling
    /// </summary>
    public float recoilSpeed;

    /// <summary>
    /// How much you will need to scroll for recoil to move to the next LevelPage
    /// </summary>
    public float recoilSensitivity = 5;

    [HideInInspector]
    /// <summary>
    /// will scroll to this position during auto-scroll State.
    /// </summary>
    public float scrollToPosition = -1f;

    [HideInInspector]
    /// <summary>
    /// The GameObject that contains the content of the ScrollRect
    /// </summary>
    public GameObject contentObject;

    [HideInInspector]
    /// <summary>
    /// the scrollBar we will be using (Horizontal/Vertical)
    /// </summary>
    public Scrollbar thisScrollBar;

    [HideInInspector]
    /// <summary>
    /// the Number of LevelPages
    /// </summary>
    public int pageCount;

    /// <summary>
    /// The dictionary that maps the scrollBar states to methods to execute.
    /// </summary>
    public Dictionary<states, Action> dictionary = new Dictionary<states, Action>();



    /*
    Generic Delegate
    --Delegates store one or more methods, and when the delegate is called all methods it was storing will be called upon.
    learn more about delegates at https://msdn.microsoft.com/en-us/library/ms173172.aspx
    */
    public delegate void genericDelegate();

    //Idle Delegates
    public genericDelegate startIdle;
    public genericDelegate duringIdle;
    public genericDelegate endIdle;

    //Scorlling Delegates
    public genericDelegate startScrolling;
    public genericDelegate duringScrolling;
    public genericDelegate endScrolling;

    //Recoil Delegates
    public genericDelegate startRecoil;
    public genericDelegate duringRecoil;
    public genericDelegate endRecoil;

    [Header("LevelData")]
    /// <summary>
    /// All the LevelData that will be mapped to the LevelButtons during the Rebuild
    /// </summary>
    public List<LD> levels = new List<LD>();

#endregion

#region MonoMethods

    void Awake()
    {
        //set the currentLevelNum to Zero -- this doesn't qualify as a level
        PlayerPrefs.SetInt("currentLevelNum",0);

        //get the ScrollRect
        ScrollRect thisScrollRect = gameObject.GetComponent<ScrollRect>();
        thisScrollRect.inertia = false;

        //do stuff based on scroll Direction
        if (direction == directions.Horizontal)
        {
            thisScrollBar = thisScrollRect.horizontalScrollbar;

            thisScrollRect.vertical = false;
            thisScrollRect.horizontal = true;

            contentObject.GetComponent<GridLayoutGroup>().startCorner = GridLayoutGroup.Corner.UpperLeft;
        }
        else
        {
            thisScrollBar = thisScrollRect.verticalScrollbar;

            thisScrollRect.vertical = true;
            thisScrollRect.horizontal = false;

            thisScrollBar.direction = Scrollbar.Direction.BottomToTop;

            if (direction == directions.Vertical_B2T)
            {
                contentObject.GetComponent<GridLayoutGroup>().startCorner = GridLayoutGroup.Corner.LowerLeft;
            }
            else if (direction == directions.Vertical_T2B)
            {
                contentObject.GetComponent<GridLayoutGroup>().startCorner = GridLayoutGroup.Corner.UpperLeft;
            }


        }
    }

    void Start()
    {
        //enable the GridLayouts
        enableGridLayouts();

        //find the contentObject, and get the pageCount
        contentObject = gameObject.transform.Find("Viewport").Find("Content").gameObject;
        pageCount = contentObject.transform.childCount;

        //can't have zero
        if (pageCount == 0)
        {
            Debug.LogError("there are no LevelPages, please rebuildLevelPages");
            return;
        }

        //find out what direction we are moving
        bool hor = (direction == directions.Horizontal)? true : false;

        //resize the contentObject
        ResizeContent(pageCount,contentObject,hor);

        //get the ScrollRect
        ScrollRect thisScrollRect = gameObject.GetComponent<ScrollRect>();
        thisScrollRect.inertia = false;

        //do stuff based on scroll Direction
        if (direction == directions.Horizontal)
        {
            thisScrollBar = thisScrollRect.horizontalScrollbar;

            thisScrollRect.vertical = false;
            thisScrollRect.horizontal = true;

            contentObject.GetComponent<GridLayoutGroup>().startCorner = GridLayoutGroup.Corner.UpperLeft;
        }
        else
        {
            thisScrollBar = thisScrollRect.verticalScrollbar;

            thisScrollRect.vertical = true;
            thisScrollRect.horizontal = false;

            thisScrollBar.direction = Scrollbar.Direction.BottomToTop;

            if (direction == directions.Vertical_B2T)
            {   
                contentObject.GetComponent<GridLayoutGroup>().startCorner = GridLayoutGroup.Corner.LowerLeft;
            }
            else if (direction == directions.Vertical_T2B)
            {
                contentObject.GetComponent<GridLayoutGroup>().startCorner = GridLayoutGroup.Corner.UpperLeft;
            }

        }

        //the current state should be Idle
        currentState = states.IDLE;

        //map the states to the methods
        dictionary.Add(states.IDLE,Idle);
        dictionary.Add(states.SCROLLING,Scrolling);
        dictionary.Add(states.RECOIL,Recoil);
        dictionary.Add(states.AUTOSCROLLING,Autoscrolling);

        //scroll to the last position
        Invoke("ScrollToLastPosition",0.001f);

        //diabling the GridLayouts will improve performance
        Invoke("disableGridLayouts",0.5f);
    }

    void Update()
    {
#if UNITY_EDITOR
        //the following will only execute while the editor is not playing
        if (EditorApplication.isPlaying)
        {
            return;
        }

        //enable GridLayouts
        enableGridLayouts();

        //renumber levels
        int ln = 1;
        foreach(LD l in levels)
        {
            l.levelNum =ln;
            ln++;
        }

        //if rebuildLevelPages or autoRebuildLevelPages
        if (rebuildLevelPages
            || autoRebuildLevelPages
            )
        {

            //get the contentObject and PageNum
            contentObject = gameObject.transform.Find("Viewport").Find("Content").gameObject;
            pageCount = contentObject.transform.childCount;

            //find the scroll direction
            bool hor = (direction == directions.Horizontal)? true : false;

            //clear/delete --before rebuild
            Clear();

            //create the LevelPages and the LevelButtons within them
            Rebuild();

            //Update pageCount;
            pageCount = contentObject.transform.childCount;

            //resize the contentObject
            ResizeContent(pageCount,contentObject,hor);

            //set to false...this is not the auto bool
            rebuildLevelPages = false;

            //let the user know the build is complete
            print("Rebuild Complete");
        }

        //delete LevelData during Rebuild if true
        if (deleteLevelData)
        {
            //delete level data from level 1 to 10000
            LevelData.ResetLevelData(10000);

            Debug.Log("data deleted");
            deleteLevelData = false;
        }
#endif
    }

    void FixedUpdate()
    {
        //call the method that is mapped to the current state
        dictionary[currentState].Invoke();

        //if the current state is Idel, and the delegate is not null, then execute the during delegate
        if (currentState == states.IDLE)
        {
            if (duringIdle != null)
            {
                duringIdle();
            }
        } //if the current state is Scrolling, and the delegate is not null, then execute the during delegate
        else if (currentState == states.SCROLLING)
        {
            if (duringScrolling != null)
            {
                duringScrolling();
            }
        } //if the current state is Recoil, and the delegate is not null, then execute the during delegate
        else if (currentState == states.RECOIL)
        {
            if (duringRecoil != null)
            {
                duringRecoil();
            }
        }

    }

#endregion

#region Methods

    //rebuild the LevelPages and the LevelButtons
    private void Rebuild()
    {
        //don't do anything if the level Count is zero
        if (levels.Count == 0)
        {
            return;
        }

        //create an emptyLevelPage that will be a template for all other LevelPages
        GameObject emptyLevelPage = new GameObject("MyGO", typeof(RectTransform));
        GridLayoutGroup GLG = emptyLevelPage.AddComponent<GridLayoutGroup>();
        GLG.constraint = levelPageConstraint;
        GLG.constraintCount = constraintCount;
        GLG.childAlignment = TextAnchor.MiddleCenter;
        GLG.cellSize = buttonSize;
        GLG.spacing = spacing;

        //create an emptyLevelButton to act as a filler for last levelPage
        emptyLevelButton =  Instantiate(levelButton);
        while(emptyLevelButton.transform.childCount > 0)
        {
            DestroyImmediate(emptyLevelButton.transform.GetChild(0).gameObject);
        }
        Component[] comps = emptyLevelButton.GetComponents<Component>();
        for(int i = 0; i < comps.Length;i++)
        {   
            if ( comps[i].GetType().Name != "RectTransform"
                && comps[i].GetType().Name != "CanvasRenderer"
                )
            {
                DestroyImmediate(comps[i]);
            } 
        }

        //set ints for loops
        //PageNum
        int pageNum = 1; 
        //levelNum
        int levelNum = 1; 
        //number of LevelButtons in the current page
        int levelsInPage = 0;

        //the currentLevelPage
        GameObject currentLevelPage = null;

        //for each items in levels
        foreach(LD l in levels)
        {
            //if the currentLevelPage has enough LevelButtons within it, 
            //or it currentLevel is Null
            //then create a new currentLevelPage
            if (
                levelsInPage == levelsPerPage
                || currentLevelPage == null
                )
            {
                currentLevelPage = GameObject.Instantiate(emptyLevelPage);
                currentLevelPage.name = "LevelPage" + pageNum.ToString();
                currentLevelPage.transform.SetParent(contentObject.transform);
                currentLevelPage.transform.localScale = new Vector3(1f,1f,1f);
                pageNum ++;
                levelsInPage = 0;
            }

            //create a new LevelButton
            GameObject lB = GameObject.Instantiate(levelButton);
            lB.name = "Level" + levelNum.ToString();

            lB.transform.SetParent(currentLevelPage.transform);
            lB.transform.localScale = new Vector3(1f,1f,1f);

            //transfer data to the LevelButton.cs
            LevelButton lBS = lB.GetComponent<LevelButton>();
            lBS.levelNum = l.levelNum;
            lBS.levelName = l.levelName;
            lBS.sceneName = l.sceneName;

            //increment
            levelNum++;
            levelsInPage++;

        }

        //fillin with emptyLevelButtons
        GameObject ELB = null;
        while (levelsInPage < levelsPerPage)
        {
            ELB = GameObject.Instantiate(emptyLevelButton);
            ELB.name = "empty";
            ELB.transform.SetParent(currentLevelPage.transform);
            ELB.transform.localScale = new Vector3(1f,1f,1f);
            levelsInPage++;
        }

        //destory temp object
        DestroyImmediate(emptyLevelPage);
        DestroyImmediate(emptyLevelButton);

    }

    //seek and destory ...all the children ...#MetalicaReference
    private void Clear()
    {
        while(contentObject.transform.childCount > 0)
        {
            DestroyImmediate(contentObject.transform.GetChild(0).gameObject);
        }
    }

    //move the scrollBar to the last position it was saved at
    public void ScrollToLastPosition()
    {
        SLP = (PlayerPrefs.HasKey("selectedLevelPage"))?  (int)(PlayerPrefs.GetFloat("selectedLevelPage") - 1) : 0;
        thisScrollBar.value = (float)SLP/ ((float)pageCount -1f);
    }

    //change the current state of the ScrollBar
    public void SetState(states NextState)
    {

        if (currentState == NextState)
        {
            return;
        }

        if (currentState == states.IDLE)
        {
            if (endIdle != null)
            {
                endIdle();
            }
        }
        else if (currentState == states.SCROLLING)
        {
            if (endScrolling != null)
            {
                endScrolling();
            }
        }
        else if (currentState == states.RECOIL)
        {
            if (endRecoil != null)
            {
                endRecoil();
            }
        }

        print("New State: " + NextState.ToString());
        currentState = NextState;

        if (currentState == states.IDLE)
        {
            if (startIdle != null)
            {
                startIdle();
            }
        }
        else if (currentState == states.SCROLLING)
        {
            if (startScrolling != null)
            {
                startScrolling();
            }
        }
        else if (currentState == states.RECOIL)
        {

            if (startRecoil != null)
            {
                startRecoil();
            }
        }

    }

    //resize the contentObject
    private void ResizeContent(int PC, GameObject CO, bool Hor)
    {
        Rect r = GetComponent<RectTransform>().rect;
        CO.GetComponent<GridLayoutGroup>().cellSize = new Vector2(r.width ,r.height);


        if (Hor)
        {
            
            CO.GetComponent<RectTransform>().sizeDelta = new Vector2( r.width * PC,r.height-5f);

#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                CO.GetComponent<RectTransform>().anchoredPosition = new Vector2((r.width/-2f)/PC,r.height/2f);
            }
            else
            {
                CO.transform.localPosition = new Vector3( 0f,0f ,0f); 
            }
#else
            CO.transform.localPosition = new Vector3( 0f,0f ,0f); 
#endif

        }
        else
        {

            CO.GetComponent<RectTransform>().sizeDelta = new Vector2( r.width - 5,r.height * PC);

#if UNITY_EDITOR

            if (EditorApplication.isPlaying)
            {
                CO.GetComponent<RectTransform>().anchoredPosition = new Vector2(r.width/-2f,(r.height/PC) * -1f);
            }
            else
            {
                CO.transform.localPosition = new Vector3( 0f,0f ,0f); 
            }

#else
            CO.transform.localPosition = new Vector3( 0f,0f ,0f); 
#endif



        }


    } 

    //disable GridLayouts
    private void disableGridLayouts()
    {
        enableGridLayouts(false);
    }

    //enable/disable the GridLayouts
    private void enableGridLayouts(bool enable = true)
    {
        contentObject.GetComponent<GridLayoutGroup>().enabled = enable;

        for(int i = 0;i < contentObject.transform.childCount; i++)
        {
            contentObject.transform.GetChild(i).gameObject.GetComponent<GridLayoutGroup>().enabled = enable;
        }
    }

    //update the selectedLevelPageFloat 
    private void UpdateSLP()
    {
        selectedLevelPageFloat = (thisScrollBar.value / (1f/(pageCount-1f))) + 1f;

        if (direction == directions.Vertical_T2B)
        {
            selectedLevelPageFloat = ( (1f - thisScrollBar.value) / (1f/(pageCount-1f))) + 1f;
        }

        if (selectedLevelPage != Mathf.RoundToInt(selectedLevelPageFloat))
        {
            selectedLevelPage = Mathf.RoundToInt(selectedLevelPageFloat);
        }

    }

    //update the selectedLevelPageFloat
    private void UpdateSLP(int value)
    {
        selectedLevelPageFloat = (float)value;

        if (direction == directions.Vertical_T2B)
        {
            selectedLevelPageFloat = (pageCount - value) + 1f;
        }

        if (selectedLevelPage != Mathf.RoundToInt(selectedLevelPageFloat))
        {
            selectedLevelPage = Mathf.RoundToInt(selectedLevelPageFloat);
        }
    }

    public void ChangePage(int pageDelta)
    {
        int pageNum = ((int)selectedLevelPage + pageDelta)-1;

        scrollToPosition = (float)pageNum/(float)(pageCount-1);

        SetState(states.AUTOSCROLLING);
    }


    //the following methods are mapped to the scrollBar states

    //this method is used while the scrollBar is Idel
    public void Idle()
    {
        //nothing
    }

    //this method is used while the scrollBar is Scrolling
    public void Scrolling()
    {
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_STANDALONE || UNITY_WEBGL
        //if the mousebutton goes up...go to Recoil
        if (Input.GetMouseButtonUp(0)) 
        {
            SetState(states.RECOIL);
        }

        //if you stop scrolling...go to Recoil
        if ( 
            Input.mouseScrollDelta == Vector2.zero
            && !Input.GetMouseButton(0)
            )
        {
            SetState(states.RECOIL);
        }
#else

#if UNITY_IOS || UNITY_IPHONE || UNITY_ANDROID || UNITY_TIZEN
        //if there is no touches...go to Recoil
        if (Input.touchCount == 0)
        {
            SetState(states.RECOIL);
        }
#endif

#endif
        UpdateSLP();
    }

    //this method is used while the scrollBar is Recoil
    public void Recoil()
    {

        //Loop to find the closest LevelPage
        
        float lowestValue = 1000f;
        int i_low = 0;

        for (int i =0 ;i < pageCount;i++)
        {

            float value  = Mathf.Abs((float)i/(float)(pageCount-1) - thisScrollBar.value );

            if (i != SLP)
            {
                value = value/recoilSensitivity;
            }

            if ( 
                value < lowestValue 
                )
            {
                lowestValue = value;
                i_low = i;
            }
        }

        //lerp to the new scroll value
        thisScrollBar.value = Mathf.Lerp(thisScrollBar.value, (float)i_low/(float)(pageCount-1) ,Time.deltaTime * recoilSpeed);

        //Update the selectedLevelPageFloat
        UpdateSLP();

        //if the values are close enough...go to Idel
        if (Mathf.Abs(thisScrollBar.value - (float)i_low/(float)(pageCount-1)) < 0.001f)
        {
            SLP = i_low;
            thisScrollBar.value = (float)i_low/(float)(pageCount-1);

            SetState(states.IDLE);
        }

    }

    //this method is used while the scrollBar is Autoscrolling
    public void Autoscrolling()
    {
        thisScrollBar.value = Mathf.Lerp(thisScrollBar.value,scrollToPosition,Time.deltaTime * recoilSpeed);

        if ( Mathf.Abs(thisScrollBar.value - scrollToPosition) < 0.001f)
        {
            SetState(states.RECOIL);
        }
    }

#endregion

#region Events
//these effects exist due to inheriting IDragHandler, IPointerDownHandler , IPointerUpHandler

    /// <summary>
    /// Raises the drag event.
    /// </summary>
    /// <param name="data">Data.</param>
    public  void OnDrag(PointerEventData data) 
    {
//        print("OnDrag");
        SetState(states.SCROLLING);
    }

    /// <summary>
    /// Raises the pointer up event.
    /// </summary>
    /// <param name="data">Data.</param>
    public  void OnPointerUp(PointerEventData data)
    {
//        print("OnPointerUp");
        SetState(states.RECOIL);
    }

    /// <summary>
    /// Raises the pointer down event.
    /// </summary>
    /// <param name="data">Data.</param>
    public  void OnPointerDown (PointerEventData data) 
    {
//        print("OnPointerDown");
//        SetState(states.SCROLLING);
    }

    /// <summary>
    /// Raises the scroll event.
    /// </summary>
    /// <param name="data">Data.</param>
    public void OnScroll (PointerEventData data) 
    {
        SetState(states.SCROLLING);
    }

#endregion

}


//this class is Just a container for the Lists Items
[System.Serializable]
public class LD
{
    public int levelNum;
    public string levelName;
    public string sceneName;

}