////////////////////////////////////////////
/// ------------------------------------ /// 
/// --�̳�ùķ��̼� ���� ���� �׽�Ʈ--- ///
/// ------------------------------------ /// 
////////////////////////////////////////////

// �⺻ ���� ������ ��.
// ��� �����̶� ��.
// C#�� using�� ���� �����̶�� ���� ��.
var app = require('express')();
var http = require('http').Server(app);
var io = require('socket.io')(http);
// �⺻ ���� ������ ��.

// ������ ������ ���� Require.
var mongoClient = require('mongodb').MongoClient;
var mongoURL = 'mongodb://localhost:27017/';

// DB �߰� ���� ������.

var myobj;  // �߰��� ������.
var mClient;
// DB �̸��� ������ ����
var NameDB = 'mydb';
// ���õ� DB.
var dbo;
// ���̵� ����� ���� ����.
var id = 0;

// ���ۼ��� 0.
// ���� Node.js ���� ��ũ��Ʈ�� �����ϱ� ���� ���� ������ �����Ͽ��� �Ѵ�.
// ������ ���������� ��ġ�� C:\Program Files\MongoDB\Server\3.6\bin\mongod.exe

// ���ۼ��� 1.
// ���α׷��� �����ϸ� ������ ���۵ǰ� Ŭ���̾�Ʈ ������ ���� �غ� ��Ŵ.
http.listen(3000, () => {
    console.log('*************************************');
    console.log('*************************************');
    console.log('**                                 **');
    console.log('** INNO Simulation Server On: 3000 **');
    console.log('**                                 **');
    console.log('*************************************');
    console.log('*************************************');
});

// ���ۼ��� 2.
// ������ ����.
// ������ �̸����� (33���� NameDB ������ �����Ǿ� �ִ� �̸�) �����ͺ��̽� ����.
mongoClient.connect(mongoURL, (err, db) => {
    if (err) throw err;
    mClient = db;
    dbo = mClient.db(NameDB);
    dbo = db.db(NameDB);
    console.log('DB Ready!');
});

// ���� Ŭ���̾�Ʈ(����)���� ������ ���۵� ��� 'connection'���� ������ �ǰ�.
// Ŀ�ؼ� ������ �ٷ� �Ʒ��� �ִ� ������� ����ǰų� Ȥ�� ����(����) ������ ���·�.
// ������ �ȴ�.
io.on('connection', (socket) => {

    console.log('Client Accessed');

    id = socket.id;
    console.log('socket.id: ' + id);

    // ������ �α���//
    socket.on('loginControl', (data, clientMethod) => {
        console.log(data);
        clientMethod('Loged In');
        //var data = JSON.parse(jsondata);
        //console.log('ID: ' + data.id + ' :::: P/W' + data.pw);
    });


    // Ŭ���̾�Ʈ �α���//
    socket.on('login', (jsondata, afn) => {
        myobj = jsondata;
        console.log(JSON.stringify(jsondata));
        afn('Loged In');
        //var data = JSON.parse(jsondata);
        //console.log('ID: ' + data.id + ' :::: P/W' + data.pw);
    });
    socket.on('insertdata', () => {


        // �ӽ÷� �־�� ���̽� ������.
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
                // �����ͺ��̽��� �߰� ������ �Է�.
                dbo.collection('customers').insertMany([myobj], (err, res) => {
                    if (err) throw err;
                    console.log('1 Document Inserted');
                    console.log("Response: %j", res);

                    console.log("socket.id: " + id);
                    io.sockets.sockets[socket.id].emit('insertdata', 'End');
                    // Close�� Ŭ���̾�Ʈ�� ������ �������� ����.
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