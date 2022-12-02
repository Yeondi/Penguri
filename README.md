# Project-Pgpg

Scripts:<br>
[F] = 폴더<br>
<br>
<br>
<br>
[F]__Lab<br>
	CheatTable.cs - 디버그용 치트 코드 <br>
	CloudCode.cs - 유니티 클라우드코드(미완)<br>
	CloudSave.cs - 유니티 클라우드세이브 (프로토타입)<br>
	GoogleCloudSave.cs - 구글 클라우드 세이브코드 (사용안함)<br>
<br>
* 이곳은 주로 개발자 재량으로 개발하고 테스트용도로 따로 빼놓은 폴더입니다.<br>
** 씬 역시 테스트용을 나누어 이곳의 스크립트를 테스트하고 있습니다.<br>
<br>
<br>
[F]_Sigleton<br>
	GameManager.cs - 게임 종합관리 <기획이 엎어지거나 기능들이 독립하면서 큰 역할이 없습니다.><br>
	ItemStatusManager.cs - 아이템 상태 관련<br>
	LoadingSceneController.cs - 로딩씬 관련<br>
	MoneyManager.cs - 인게임 재화<br>
	Penguri.cs - 펭귄 종합<br>
	PopUpController.cs - 팝업 컨트롤러<br>
	QuestHandler.cs - 퀘스트 전반<br>
	Reward.cs - 보상<br>
	UserDataManager.cs - 세이브/로드 유저데이터 관리<br>
<br>
* 싱글턴 디자인패턴 모아놓은 폴더<br>
** 초기엔 GameManager에서 전부 호출하는 방식을 사용하다가 유지보수의 어려움을 우려해서 필요한것만 떼어서 독립시켜버림.<br>
<br>
<br>
[F]Item<br>
	Item.cs - 최상위 스크립트 / 아이템관련 공통<br>
	Food.cs - 음식 탭<br>
	WarmItem.cs - 보온아이템 탬<br>
	GlobalWarming.cs - 지구온난화 탭<br>
<br>
* 위 세개는 아이템마다 개별로 붙어있음.<br>
<br>
[F]Observer<br>
	Observer.cs - 가상클래스,함수<br>
	ISubject.cs - Subject Interface<br>
	QuestObserver.cs - 퀘스트용 옵저버<br>
	Broadcast.cs - Add된 옵저버를 대상으로 신호를 퍼트려 줄 클래스<br>
<br>
* 앞으로 무언가 바뀌더라도 수정할 필요 없음.<br>
** 단, 옵저버 용도 추가의 경우 새로운 스크립트 만든 후 Broadcast에서 AddObserver해야함.<br>
<br>
[F]UI_Scripts<br>
	CaptureMode.cs - 캡처용 코드. persistentPath쪽 경로로 잡아놓았으며, 안드로이드의 경우 갤러리 refresh까지 추가해놓음.<br>
	HideUI.cs - UI숨기기 코드.<br>
<br>
* Top_Panel의 UI관련 스크립트 모음<br>
<br>
그 외<br>
<br>
TSVLoader.cs - 받아온 데이터를 딕셔너리에 키값과 매칭해서 넣는 코드<br><br>
GoogleSheetManager.cs - 구글시트에서 받아오는 코드<br>
<br>
Att_Properties.cs - 출석 속성<br>
Attendance.cs - 출석<br>
NTP.cs - 타임치트 방지용으로 만든 코드. 구글서버에서 시간 받아와서 출석치트나 시간관련 치트 방지<br>
<br>
BuffController.cs - 아픔,기쁨 등등 버프용<br>
EventController.cs - 날씨이벤트 등<br>
EventType.cs<br>
<br>
ChangeAmount_Food.cs - 아이템 개수 변경용<br>
ChangeColor.cs - 엄청 옛날 디버그용 코드<br>
DragEvent.cs - 펭귄 터치하거나 드래그할때 반응<br>
<br>
<br>
PlayGamesConnect.cs - 구글 플레이관련 스크립트 모아놓을 예정 (리더보드 등)<br>
TouchToStart.cs - 터치해서 시작하는 스크립트. 별거없는데 유지보수 편하게하려고 게임매니저에서 떼어왔습니다. <br>
<br>
* 관련있는 코드는 묶어놓음<br>
** 향후 개발자님 판단하에 수정하시면 됩니다.<br>
*** 대부분 초기코드라 수정 혹은 최적화가 필요할 수 있음.<br>
<br>
<br>
<h1> 확대 가능합니다. <br>
<img width="80%" src="https://user-images.githubusercontent.com/51913393/204423551-0a07ab79-3c12-455b-8797-73824f0ad7bc.png"/>
