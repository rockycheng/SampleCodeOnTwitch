Feature: LoginOnInstagramTestPlan
	In order to confirm the candidate Rocky Cheng's' experience.
	As a NetBase interviewer
	We support the off-site testing,  testing of the login feature for https://www.instagram.com/

@Login
Scenario: 00_001_Confirm the Website Instagram Index page
	Given Enter the INSTAGRAM web site url
	Then ConfirmWebSite /

@Login
Scenario: 00_002_Confirm the user account input fields on login 
	Given Click Button_Login By cssSelector
	When Input InputAccount On Input_UserAccount By cssSelector
	Then Button_Submit_Login Clickable is false

@Login
Scenario: 00_003_Confirm the password input fields on login 
	Given Clean Input_UserAccount By cssSelector
	When Input InputPassword On Input_Password By cssSelector
	Then Button_Submit_Login Clickable is false
	
@Login
Scenario: 00_004_Confirm the login fail message when user typing the wrong account
	Given Clean Input_Password By cssSelector
	And Input tagy04 On Input_UserAccount By cssSelector
	And Input InputPassword On Input_Password By cssSelector
	When Click Button_Submit_Login By cssSelector
	Then Confirm ErrorMessage_LoginFailed Display Theusernameyouentereddoesn'tbelongtoanaccount.Pleasecheckyourusernameandtryagain. By cssSelector

@Login
Scenario: 00_005_Confirm the login fail message when user typing the wrong password
	Given Clean Input_UserAccount By cssSelector
	And Clean Input_Password By cssSelector
	And Input InputAccount On Input_UserAccount By cssSelector
	And Input 1234567890 On Input_Password By cssSelector
	When Click Button_Submit_Login By cssSelector
	Then Confirm ErrorMessage_LoginFailed Display Sorry,yourpasswordwasincorrect.Pleasedouble-checkyourpassword. By cssSelector