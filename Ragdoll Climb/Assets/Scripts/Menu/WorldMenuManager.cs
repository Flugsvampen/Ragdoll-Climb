﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class WorldMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainGroup;
    [SerializeField] GameObject playerSelectGroup;
    [SerializeField] GameObject singlePlayerSelectGroup;
    [SerializeField] GameObject levelSelectGroup;
	[SerializeField] GameObject howToPlayGroup;
    [SerializeField] GameObject diffLengthGroup;
	[SerializeField] GameObject optionsGroup;
    [SerializeField] GameObject creditsGroup;

	[SerializeField] float cameraSpeed = 0.5f;
	[SerializeField] GameObject mainCamera;

    [SerializeField] LevelLoader levelLoader;

	[SerializeField] EventSystem eventSystem;

	bool moving = false;

    // The path of the navigation of the menu
    public Stack<GameObject> groupPath = new Stack<GameObject>();

	[SerializeField]GameObject lastGroup;

    PlayerIndex[] playerIndexes = new PlayerIndex[4];
    GamePadState[] states = new GamePadState[4];
    GamePadState[] prevStates = new GamePadState[4];


    private void Start ()
    {
        // Defaults to main group in case we forget to switch all groups in editor
        mainGroup.SetActive(true);
        playerSelectGroup.SetActive(false);
        levelSelectGroup.SetActive(false);
        singlePlayerSelectGroup.SetActive(false);
        howToPlayGroup.SetActive(false);
        diffLengthGroup.SetActive(false);
        optionsGroup.SetActive(false);
        creditsGroup.SetActive(false);

        groupPath.Push(mainGroup);

        playerIndexes[0] = PlayerIndex.One;
        playerIndexes[1] = PlayerIndex.Two;
        playerIndexes[2] = PlayerIndex.Three;
        playerIndexes[3] = PlayerIndex.Four;

        PlayerInfoSingleton.instance.Load();

        eventSystem.SetSelectedGameObject(groupPath.Peek().GetComponentInChildren<Button>().gameObject);    //  TEST !!!!!!!!!!
    }


    private void Update()
    {
        // Checks input from all four controllers
        for (int i = 0; i < playerIndexes.Length; i++)
        {
            prevStates[i] = states[i];
            states[i] = GamePad.GetState(playerIndexes[i]);

            // If B is pressed and the current menu group isn't the main one
            if (states[i].Buttons.B == ButtonState.Pressed && prevStates[i].Buttons.B == ButtonState.Released && groupPath.Peek() != mainGroup && !moving)
                Back();

            if (eventSystem.currentSelectedGameObject == null && groupPath.Peek() != playerSelectGroup && groupPath.Peek() != singlePlayerSelectGroup)
            {
                if (((states[i].DPad.Down == ButtonState.Pressed && prevStates[i].DPad.Down == ButtonState.Released) || (states[i].DPad.Up == ButtonState.Pressed && prevStates[i].DPad.Up == ButtonState.Released) || (states[i].DPad.Left == ButtonState.Pressed && prevStates[i].DPad.Left == ButtonState.Released) || (states[i].DPad.Right == ButtonState.Pressed && prevStates[i].DPad.Right == ButtonState.Released) || (states[i].Buttons.A == ButtonState.Pressed && prevStates[i].Buttons.A == ButtonState.Released) || (states[i].ThumbSticks.Left.Y > 0f || states[i].ThumbSticks.Left.Y < 0f && prevStates[i].ThumbSticks.Left.Y == 0f)))
                {
                    eventSystem.SetSelectedGameObject(groupPath.Peek().GetComponentInChildren<Button>().gameObject);
                }
            }
        }
		
        if(eventSystem.currentSelectedGameObject == null && moving == false && groupPath.Count == 1)
        {
            //eventSystem.SetSelectedGameObject(groupPath.Peek().GetComponentInChildren<Button>().gameObject);

        }

		Vector3 camGoalPos = groupPath.Peek().transform.GetChild(0).transform.position;

		mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, camGoalPos, cameraSpeed);   // BÖR VARA DELTA TIME HÄR ########¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤%%%%%%%%%%%%%%%%%%%%%%%%%&
		mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, groupPath.Peek().transform.GetChild(0).transform.rotation, cameraSpeed);

		if (Vector3.Distance(mainCamera.transform.position, camGoalPos) <= 1f && moving)
		{
			lastGroup.SetActive(false);
			eventSystem.enabled = true;
			moving = false;
            
            if (groupPath.Peek() == playerSelectGroup || groupPath.Peek() == singlePlayerSelectGroup)
				eventSystem.SetSelectedGameObject(null);
            else
			    eventSystem.SetSelectedGameObject(groupPath.Peek().GetComponentInChildren<Selectable>().gameObject);
        }

		if (Input.GetKeyDown("b") && eventSystem.currentSelectedGameObject == null)
        {
			eventSystem.SetSelectedGameObject(groupPath.Peek().GetComponentInChildren<Button>().gameObject);
		}
	}


    public void OpenMenuGroup(GameObject group)
    {
        eventSystem.SetSelectedGameObject(null);
        //groupPath.Peek().SetActive(false);
        eventSystem.enabled = false;
		lastGroup = groupPath.Peek();
        groupPath.Push(group);
        //eventSystem.SetSelectedGameObject(group.GetComponentInChildren<Button>().gameObject);
        group.SetActive(true);

		moving = true;

        if (group == playerSelectGroup)
            playerSelectGroup.GetComponent<Lobby>().ResetValues();
        else if (group == diffLengthGroup)
            diffLengthGroup.GetComponent<DiffLengthSelection>().ResetValues();
    }


    public void Back()
    {
		//groupPath.Pop().SetActive(false);
		//eventSystem.SetSelectedGameObject(groupPath.Peek().GetComponentInChildren<Button>().gameObject);
		eventSystem.enabled = false;
		lastGroup = groupPath.Peek();
		groupPath.Pop();
		groupPath.Peek().SetActive(true);
        moving = true;

        eventSystem.SetSelectedGameObject(null);// TEST!!!!!
	}


    public void LoadLevel()
    {
        levelLoader.LoadLevelAsync(PlayerInfoSingleton.instance.selectedLevel);
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
