using UnityEngine;

public class TabsManager : MonoBehaviour {
    [SerializeField]
    private GameObject _mainScreen;
    
    private int _openedTabsCount;

    public void OpenTab(bool isOn) {
        if(isOn) _openedTabsCount++;
        else _openedTabsCount--;
        
        UpdateMainScreen();
    }

    private void UpdateMainScreen() {
        _mainScreen.SetActive(_openedTabsCount == 0);
    }
}