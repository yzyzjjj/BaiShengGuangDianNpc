﻿$(function () {
    load();
});

var op = function (data, type, row) {
    var html = '<button type="button" class="btn btn-primary" data-toggle="modal"  onclick="showDevice(\'' +
        row.DeviceId +
        '\',\'' +
        row.ServerId +
        '\',\'' +
        row.Ip +
        '\',\'' +
        row.Port +
        '\',\'' +
        row.Monitoring +
        '\',\'' +
        row.Frequency +
        '\',\'' +
        row.Instruction +
        '\',\'' +
        row.Storage +
        '\')">操作</button>';
    return html;
};

function load() {
    $.get("/gate/device/list", function (res) {
        $("#deviceTable").empty();
        $("#deviceTable")
            .DataTable({
                "destroy": true,
                "paging": true,
                "searching": true,
                "data": res.datas,
                "aLengthMenu": [20, 40, 60], //更改显示记录数选项  
                "iDisplayLength": 20, //默认显示的记录数  
                "columns": [
                    { "data": null, "title": "操作", "render": op },
                    { "data": "DeviceId", "title": "设备ID" },
                    { "data": "Code", "title": "机台号" },
                    { "data": "State", "title": "连接状态" },
                    { "data": "DeviceState", "title": "设备状态" },
                    { "data": "ServerId", "title": "NpcServerId" },
                    { "data": "Ip", "title": "ip" },
                    { "data": "Port", "title": "端口" },
                    { "data": "Monitoring", "title": "是否监控" },
                    { "data": "Frequency", "title": "监控频率" },
                    { "data": "Storage", "title": "是否存储" },
                    { "data": "Instruction", "title": "指令" },
                ],
            });
    });
}

function showDevice(deviceId, serverId, ip, port, monitoring, frequency, instruction, storage) {
    $("#deviceId").html(deviceId);
    $("#serverId").html(serverId);
    $("#ip").val(ip);
    $("#port").val(port);
    $("#monitoring").val(monitoring);
    $("#frequency").val(frequency);
    $("#instruction").val(instruction);
    $("#storage").val(storage);
    $("#detailInfo").modal("show");
}

function btnStorage() {
    var data = [{
        deviceId: $("#deviceId").text(),
        serverId: $("#serverId").text(),
        ip: $("#ip").val(),
        port: $("#port").val(),
        monitoring: $("#monitoring").val(),
        frequency: $("#frequency").val(),
        storage: $("#storage").val(),
        instruction: $("#instruction").val(),
    }];
    $.post("/gate/device/batchsetstorage",
        {
            devicesList: JSON.stringify(data)
        }, function (res) {
            var s = "";
            if (data.length == 1) {
                s = res.datas[0].errmsg;
            } else {
                for (var i = 0; i < data.length; i++) {
                    s += "设备ID:" + res.datas[i].DeviceId + "," + res.datas[i].errmsg + ";";
                }
            }
            alert(s);
            load();
        });
};

function btnMonitoring() {
    var data = [{
        deviceId: $("#deviceId").text(),
        serverId: $("#serverId").text(),
        ip: $("#ip").val(),
        port: $("#port").val(),
        monitoring: $("#monitoring").val(),
        frequency: $("#frequency").val(),
        instruction: $("#instruction").val(),
        storage: $("#storage").val(),
    }];
    $.post("/gate/device/batchsetfrequency",
        {
            devicesList: JSON.stringify(data)
        }, function (res) {
            var s = "";
            if (data.length == 1) {
                s = res.datas[0].errmsg;
            } else {
                for (var i = 0; i < data.length; i++) {
                    s += "设备ID:" + res.datas[i].DeviceId + "," + res.datas[i].errmsg + ";";
                }
            }
            alert(s);
            load();
        });
}

function btnSend() {
    $("#text").empty();
    var data = [{
        deviceId: $("#deviceId").text(),
        serverId: $("#serverId").text(),
        ip: $("#ip").val(),
        port: $("#port").val(),
        monitoring: $("#monitoring").val(),
        frequency: $("#frequency").val(),
        instruction: $("#instruction").val(),
        storage: $("#storage").val(),
    }];
    $("#ins").html($("#instruction").val());
    $.post("/gate/device/batchsendback",
        {
            devicesList: JSON.stringify(data)
        }, function (res) {
            var s = "";
            if (data.length == 1) {
                s = res.messages[0].Item2;
            } else {
                for (var i = 0; i < data.length; i++) {
                    s += "设备ID:" + res.messages[i].Item1 + "," + res.datas[i].Item2 + ";";
                }
            }
            $("#text").append(s);
        });
}

function btnAnalysis() {
    var len = 3;
    for (var j = 0; j < len; j++) {
        $("#d" + (j + 1)).empty();
    }
    var text = $("#text").text();
    if (text == "" || text == null) {
        return;
    }
    var msg = JSON.parse(text);
    for (var j = 0; j < 3; j++) {
        $("#d" + (j + 1)).empty();
        var v = new Array();
        if (j == 0) {
            v = msg.vals;
        } else if (j == 1) {
            v = msg.ins;
        } else if (j == 2) {
            v = msg.outs;
        } else {
            break;
        }
        var i = 0;
        for (var n in v) {
            $("#d" + (j + 1)).append(n + " : " + v[n] + "       ");
            i++;
            if (i == 10) {
                $("#d" + (j + 1)).append("\n");
                i = 0;
            }
        }
    }
}