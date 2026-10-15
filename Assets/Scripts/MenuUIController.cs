using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using DG.Tweening;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour {

    public static MenuUIController instance;

    [SerializeField]
    private GameObject _pauseBtn;
    [SerializeField]
    private GameObject _story;
    [SerializeField]
    private GameObject _itemBtn;
    [SerializeField]
    private GameObject _menuBtn;
    [SerializeField]
    private GameObject _scoreText;
    [SerializeField]
    private GameObject _logoParent;
    [SerializeField]
 //   private GameObject _logo;
//    [SerializeField]
    private GameObject _gameOverMenu;
    [SerializeField]
    private GameObject _topBar, _midBar, _botBar;
    [SerializeField]
    private GameObject _gotItemPanel;
    [SerializeField]
    private GameObject _photoShare;

    public bool isOpenGameOver = false;
    // 初始化组件
    private void Awake()
    {

        MenuUIController.instance = this;
    }
    private void Start()
    {

    }
    public void ShowLogo()
    {
        _logoParent.SetActive(true);
//        _logo.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "start", false);
    }
    public void HideLogo()
    {
        StartCoroutine(HideLogoExecute());
    }
    IEnumerator HideLogoExecute()
    {
 //       _logo.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "end", false);
        yield return new WaitForSeconds(0f);
        _logoParent.SetActive(false);

    }
    public void OpenMenu()
    {
        //Debug.Log("打开菜单");
        if (SceneManager.instance.currentState != SceneManager.State.trainArrived)
        {
            SceneManager.instance.ChangeState(SceneManager.State.trainArrived);
        }
        _pauseBtn.SetActive(false);
        _story.SetActive(true);
        _itemBtn.SetActive(true);
        _menuBtn.SetActive(true);
        _scoreText.SetActive(false);
        _gameOverMenu.SetActive(false);
        _gotItemPanel.SetActive(false);
        if (SceneManager.instance.gettingItem || ItemManager.instance.gotItemOpen)
        {
            SceneManager.instance.gettingItem = false;
            ItemManager.instance.gotItemOpen = false;
        }
        _story.GetComponent<CanvasGroup>().DOFade(1f,0.5f);
        _itemBtn.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
        _menuBtn.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
    }
    public void CloseMenuFirstStart()
    {
        //Debug.Log("首次关闭菜单");
        _scoreText.gameObject.SetActive(true);
        _gameOverMenu.gameObject.SetActive(false);
        _gotItemPanel.gameObject.SetActive(false);
        _story.gameObject.SetActive(false);
        _itemBtn.gameObject.SetActive(false);
        _menuBtn.gameObject.SetActive(false);
    }
    public void CloseMenu()
    {
        //Debug.Log("关闭菜单");
        _scoreText.gameObject.SetActive(true);
        _gameOverMenu.gameObject.SetActive(false);
        _gotItemPanel.gameObject.SetActive(false);
        _pauseBtn.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
        _story.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).OnComplete(()=> { _story.gameObject.SetActive(false); });
        _itemBtn.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).OnComplete(() => { _itemBtn.gameObject.SetActive(false); });
        _menuBtn.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).OnComplete(() => { _menuBtn.gameObject.SetActive(false); });
    }

    public void OpenGameOver()
    {
        //Debug.Log("游戏结束");
        isOpenGameOver = true;
        _scoreText.gameObject.SetActive(true);
        _gameOverMenu.gameObject.SetActive(true);
        _pauseBtn.SetActive(false);
        _story.SetActive(true);
        _itemBtn.SetActive(true);
        _menuBtn.SetActive(true);
        _gotItemPanel.SetActive(false);
        _photoShare.transform.DOLocalMoveY(150, 1f);
        _pauseBtn.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).OnComplete(() => { _pauseBtn.gameObject.SetActive(false); });
        _scoreText.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
        _story.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
        _itemBtn.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
        _menuBtn.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
        _topBar.transform.DOLocalMoveX(0, 0.5f);
        _midBar.transform.DOLocalMoveX(0, 0.65f);
        _botBar.transform.DOLocalMoveX(0, 0.75f).OnComplete(()=> { if (SceneManager.instance.currentState == SceneManager.State.gotItem) {
                SceneManager.instance.currentState = SceneManager.State.gameOver;
                SceneManager.instance.gettingItem = false;
                ItemManager.instance.gotItemOpen = false;
                NpcManager.instance.ContinueAll();
            }  });
    }
    public void CloseGameOver()
    {
        //Debug.Log("关闭结算界面");
        _scoreText.gameObject.SetActive(false);
        _pauseBtn.SetActive(false);
        _gotItemPanel.SetActive(false);
        _scoreText.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).OnComplete(() => { _scoreText.gameObject.SetActive(false); });
        _story.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).OnComplete(() => { _story.gameObject.SetActive(false); });
        _itemBtn.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).OnComplete(() => { _itemBtn.gameObject.SetActive(false); });
        _menuBtn.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).OnComplete(() => { _menuBtn.gameObject.SetActive(false); });
        _topBar.transform.DOLocalMoveX(-810f, 0.75f).OnComplete(() => { isOpenGameOver = false; _gameOverMenu.gameObject.SetActive(false); });
        _midBar.transform.DOLocalMoveX(-810f, 0.65f);
        _botBar.transform.DOLocalMoveX(-810f, 0.5f);
    }

    public void RestartMenu()
    {
        //Debug.Log("重置菜单");
        _scoreText.gameObject.SetActive(true);
        _pauseBtn.SetActive(true);
        _gotItemPanel.SetActive(false);
        _photoShare.transform.DOLocalMoveY(350, 0.5f);
        _pauseBtn.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
        _scoreText.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
        _story.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).OnComplete(() => { _story.gameObject.SetActive(false); });
        _itemBtn.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).OnComplete(() => { _itemBtn.gameObject.SetActive(false); });
        _menuBtn.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).OnComplete(() => { _menuBtn.gameObject.SetActive(false); });
        _topBar.transform.DOLocalMoveX(-810f, 0.75f).OnComplete(()=> { isOpenGameOver = false; _gameOverMenu.gameObject.SetActive(false); });
        _midBar.transform.DOLocalMoveX(-810f, 0.65f);
        _botBar.transform.DOLocalMoveX(-810f, 0.5f);
    }
}
