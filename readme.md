# Programmer automation demo

Requirements
* node.js (latest)
* Asp.Net Core version 2 (or higher)
* VSCode + C# extension
* json-server: https://github.com/typicode/json-server


To run angular client

1. Browese to `<root>\src\web-ui\programmer-webui` directory 
 * run `npm install`
 * run `ng serve`

2. Browse to <root>\db\ directory 
 * make a copy of init_db.json and name is db.json
 * Open shell and execute this command: json-server -w db.json -p 3004 | Tee json-server.log

3. Browse to http://localhost:4200

## Testing Concept
* We can describe the communication between Rest client and it's Rest server by defining unique HTTP REQUEST sequence
* In system-level Each sequence describes use case
* In (our scope) Restful HTTP request can is unique using the following request's attributes
 * Uri - request's address (base uri and + query)
 * HTTP-Method - Get/Post/Put...
 * SentRequest's body - json contained in body

## Test steps
* Triggering use case (by performing web-ui functionality)
* Examine system's artifacts

## Objective Evidence Artifacts:
At end of execution the following artifacts would be verified
* <root>\db\db.json - all the data from/to rest server
* <root>\db\json-server.log - all http requests the application made
* If auto script would trigger UI behavior (currently out-of-scope): test assertion points, execution log and screen-shots


## Rest Client Communication Model
																
											┌───────────────────┐
┌───────────────┐							│					│										┌───────────────┐
│				│							│	Rest Client		│										│				│
│	Web Socket	│	──	Notifications──►	│	(mobile, script	│	◄── 2. Http request/response ──►	│	Rest Server	│
│				│							│	web form...)	│										│				│
└───────────────┘							│					│										└───────────────┘
											└───────────────────┘										


## Test Flow Description
											┌───────────┐										┌───────────────────────┐
┌───────────────┐							│			│										│						│
│				│	──	1. Set & Click	──►	│			│										│	Test Rest-Server	│
│	Test Script	│							│	angular	│	◄──	2. Rest request/response	──►	│		(json-server)	│
│	(Selenium)	│	──	3. Assert CSS	──►	│			│										│						│
│				│							│			│										│						│
└───────────────┘							└───────────┘										└───────────────────────┘
		▲																									│
		│																									│
		│																									▼
		│																							┌───────────────┐
		└───────────────────────────────────────	4. Examine Artifacts	──────────────────────►	│	Artifacts	│
																									└───────────────┘

## Programmer Web-UI Full test cycle
__Angular Module__
1. 'primitive' directive is created (example treatment edit: contains legal ranges, text-box, backgraound)
 - Unit tests examine the directive functionality (set out-of-range value set css...)
2. 'master'directive is created (set treatment values screen)
 - Unit test for master diretive

__Angular App__
1. Web-page is created in ng app presenting the logic using module
2. system is delpoyed, connected to test Rest server
3. Selenium 'clicks' its way through use case
4. Ui is examined (css)
5. Test Rest server artifacts are examined