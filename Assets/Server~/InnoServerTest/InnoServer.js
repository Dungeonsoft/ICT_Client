////////////////////////////////////////////
/// ------------------------------------ /// 
/// --이노시뮬레이션 서버 개발 테스트--- ///
/// ------------------------------------ /// 
////////////////////////////////////////////

// 기본 세팅 무조건 들어감.
// 모듈 추출이라도 함.
// C#의 using과 같은 역할이라고 보면 됨.
var app = require('express')();
var http = require('http').Server(app);
var io = require('socket.io')(http);
// 기본 세팅 무조건 들어감.

// 몽고디비 설정을 위한 Require.
var mongoClient = require('mongodb').MongoClient;
var mongoURL = 'mongodb://localhost:27017/';

// DB 추가 관련 변수들.

var myobj;  // 추가할 데이터.
var mClient;
// DB 이름을 지정할 변수
var NameDB = 'mydb';
// 선택된 DB.
var dbo;
// 아이디 기억을 위한 변수.
var id = 0;

// 시작순서 0.
// 현재 Node.js 서버 스크립트를 실행하기 전에 먼저 몽고디비를 실행하여야 한다.
// 몽고디비 실행파일의 위치는 C:\Program Files\MongoDB\Server\3.6\bin\mongod.exe

// 시작순서 1.
// 프로그램을 시작하면 서버가 시작되고 클라이언트 접속을 받을 준비를 시킴.
http.listen(3000, () => {
    console.log('*************************************');
    console.log('*************************************');
    console.log('**                                 **');
    console.log('** INNO Simulation Server On: 3000 **');
    console.log('**                                 **');
    console.log('*************************************');
    console.log('*************************************');
});

// 시작순서 2.
// 몽고디비 시작.
// 지정된 이름으로 (33번줄 NameDB 변수에 설정되어 있는 이름) 데이터베이스 생성.
mongoClient.connect(mongoURL, (err, db) => {
    if (err) throw err;
    mClient = db;
    dbo = mClient.db(NameDB);
    dbo = db.db(NameDB);
    console.log('DB Ready!');
});

// 최초 클라이언트(개별)에서 접속이 시작될 경우 'connection'으로 연결이 되고.
// 커넥션 성공시 바로 아래에 있는 내용들이 실행되거나 혹은 실행(접속) 가능한 상태로.
// 유지가 된다.
io.on('connection', (socket) => {

    console.log('Client Accessed');

    id = socket.id;
    console.log('socket.id: ' + id);

    // 통제툴 로그인//
    socket.on('loginControl', (data, clientMethod) => {
        console.log(data);
        clientMethod('Loged In');
        //var data = JSON.parse(jsondata);
        //console.log('ID: ' + data.id + ' :::: P/W' + data.pw);
    });


    // 클라이언트 로그인//
    socket.on('login', (jsondata, afn) => {
        myobj = jsondata;
        console.log(JSON.stringify(jsondata));
        afn('Loged In');
        //var data = JSON.parse(jsondata);
        //console.log('ID: ' + data.id + ' :::: P/W' + data.pw);
    });
    socket.on('insertdata', () => {


        // 임시로 넣어보는 제이슨 데이터.
        //myobj = [{
        //    'name': 'John',
        //    'skills': ['SQL', 'C#', 'Azure']
        //}];
        var uidTxt = { 'uid': myobj.uid };
        console.log(uidTxt);
        console.log("uidTxt: "+uidTxt);

        dbo.collection('customers').findOne(uidTxt, (err,res) => {
            console.log("Response: "+ res);

            if (res == null) {
                // 데이터베이스에 추가 데이터 입력.
                dbo.collection('customers').insertMany([myobj], (err, res) => {
                    if (err) throw err;
                    console.log('1 Document Inserted');
                    console.log("Response: %j", res);

                    console.log("socket.id: " + id);
                    io.sockets.sockets[socket.id].emit('insertdata', 'End');
                    // Close는 클라이언트를 완전히 끊을때만 쓰자.
                    //mClient.close();
                });
            }
            else {
                console.log('Use Other Uid');
            }
        });
    });

    var readyCnt = 0;
    socket.on('ReadyExam', () => {
        readyCnt++;
        if (readyCnt >= 10) {
            console.log('readyCnt: ' + readyCnt+' ::: Users Are Full!');
            socket.emit('ReadyExam', readyCnt);
            readyCnt = 0;
        }
        else {
            console.log('readyCnt: ' + readyCnt);
            socket.emit('AddCnt', readyCnt);
        }
    });
});