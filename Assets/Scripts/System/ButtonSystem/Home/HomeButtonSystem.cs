using System;
using UnityEngine;
using UnityEngine.UI;
using RPGCreateNow_Local.UseCase;
using UnityEngine.EventSystems;


namespace RPGCreateNow_Local.System
{
    public class HomeButtonSystem
    {
        public GameObject SelectBox;
        public EventSystem eventSystem;
        string[] setWord; 
        
        private void SelectButtonReset()
        {
            for (int i = 0; i < SelectBox.transform.childCount; i++)
            {
                Transform childTransform = SelectBox.transform.GetChild(i);
                childTransform.gameObject.SetActive(true);
                childTransform.GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }

        void SetWord_SelectHomeAction(int selectHomeActionNum)
        {
            setWord = new string[selectHomeActionNum];
            for (int i = 0; i < selectHomeActionNum; i++)
            {
                switch ((SelectHomeAction)i)
                {
                    case SelectHomeAction.search:
                        setWord[i] = "íTçı";
                        break;
                    default:
                        break;
                }
            }
        }
        void SetWord_SearchAreaName(int searchAreaNum)
        {
            setWord = new string[searchAreaNum];
            for (int i = 0; i < searchAreaNum; i++)
            {
                if (searchAreaNum - 1 == i)
                {
                    setWord[i] = "Ç‡Ç«ÇÈ";
                }
                switch ((SearchAreaNames)i)
                {
                    case SearchAreaNames.meadow:
                        setWord[i] = "ëêå¥";
                        break;
                    default:
                        break;
                }
            }
        }
        void SetWord_SearchAreaNum(SearchAreaNames areaNames)
        {
            setWord = new string[SelectBox.transform.childCount];
            for (int i = 0; i < SelectBox.transform.childCount; i++)
            {
                if (i < 5)
                    setWord[i] = $"{(int)areaNames + 1} - {i + 1}";
                else if (i == 5)
                {
                    setWord[i] = "Ç‡Ç«ÇÈ";
                }
                //else
                //{
                //    setWord[i] = "Ç¬Ç¨Ç÷";
                //}
            }
        }

        void SetButton_SearchArea()
        {
            SelectButtonReset();
            int selectHomeActionNum = Enum.GetValues(typeof(SearchAreaNames)).Length;
            SetWord_SearchAreaName(selectHomeActionNum + 1);
            for (int i = 0; i < SelectBox.transform.childCount; i++)
            {
                Transform childTransform = SelectBox.transform.GetChild(i);
                if (i >= selectHomeActionNum + 1)
                {
                    childTransform.gameObject.SetActive(false);
                }
                else
                {
                    childTransform.GetChild(0).GetComponent<Text>().text = setWord[i];

                    if (i == selectHomeActionNum)
                        childTransform.GetComponent<Button>().onClick.AddListener(SetSelectHomeButton);
                    else
                        childTransform.GetComponent<Button>().onClick.AddListener(SetButtonEvetn_SearchArea);

                }
            }
        }

        private void SetButtonEvent_SelectHome()
        {
            string word = eventSystem.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text;
            switch (word)
            {
                case "íTçı":
                    SetButton_SearchArea();
                    break;
                default:
                    break;
            }
        }
        private void SetButtonEvetn_SearchArea()
        {
            SelectButtonReset();
            string word = eventSystem.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text;
            SearchAreaNames searchAreaNames = default;
            switch (word)
            {
                case "ëêå¥":
                    searchAreaNames = SearchAreaNames.meadow;
                    break;
                default:
                    break;
            }
            SetWord_SearchAreaNum(searchAreaNames);
            for (int i = 0; i < SelectBox.transform.childCount; i++)
            {
                Transform childTransform = SelectBox.transform.GetChild(i);
                childTransform.GetChild(0).GetComponent<Text>().text = setWord[i];
                if (i < 5)
                    childTransform.GetComponent<Button>().onClick.AddListener(
                        () =>
                        {
                            SceneChangeSystem sceneChangeSystem = new SceneChangeSystem();
                            sceneChangeSystem.SceneChange(SceneNameS.Battle);
                        }
                        );
                else if (i == 5)
                    childTransform.GetComponent<Button>().onClick.AddListener(SetButton_SearchArea);
                else
                    childTransform.gameObject.SetActive(false);
            }
        }

        public void SetSelectHomeButton()
        {
            SelectButtonReset();
            int selectHomeActionNum = Enum.GetValues(typeof(SelectHomeAction)).Length;
            SetWord_SelectHomeAction(selectHomeActionNum);
            for (int i = 0; i < SelectBox.transform.childCount; i++)
            {
                Transform childTransform = SelectBox.transform.GetChild(i);
                if (i >= selectHomeActionNum)
                {
                    childTransform.gameObject.SetActive(false);
                }
                else
                {
                    childTransform.GetChild(0).GetComponent<Text>().text = setWord[i];
                    childTransform.GetComponent<Button>().onClick.AddListener(SetButtonEvent_SelectHome);
                }
            }
        }
    }
}
