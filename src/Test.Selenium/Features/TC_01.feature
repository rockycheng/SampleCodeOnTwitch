Feature: TC_01
	In order to confirm the specific video play function
	As a swag candidate 
	Rocky Cheng supports automated test script for https://www.twitch.tv/

@Twitch
Scenario: 01_000_Confirm_The_Index_Page
	Given Enter the TWITCH web site url
	Then ConfirmWebSite /

@Twitch
Scenario: 01_001_Confirm_The_Specific_Video_Can_Play
	Given Input Monster Hunter World On Input_Search By cssSelector
	Given Click Button_Search By cssSelector
	Given ScrollDownToBottom
	Given Click Button_ShowMoreVideo By cssSelector
	Given ScrollDownToBottom
	Given Click Button_ShowAllVideo By cssSelector
	Given ClickLatestVideo On The_Elegist
	Given Snapshot Warn_Accept page
	Given Click Warn_Accept By cssSelector
	Given Snapshot Video_Stream_Zero_Sceond page
	Given Wait For 5 seconds
	Then Snapshot Video_Stream_Five_Sceond page
	Then Quit_browserAndDeleteChromeDriverTempFolder
	
	
