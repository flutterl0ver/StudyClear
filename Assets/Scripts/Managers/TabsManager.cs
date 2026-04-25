using UnityEngine;

public class TabsManager : MonoBehaviour {
    private int _openedTabsCount;

    public void OpenTab(bool isOn) {
        if(isOn) _openedTabsCount++;
        else _openedTabsCount--;
        
        UpdateMainScreen();
    }

    private void UpdateMainScreen() {
        MainScreen.Instance.gameObject.SetActive(_openedTabsCount == 0);
    }
}