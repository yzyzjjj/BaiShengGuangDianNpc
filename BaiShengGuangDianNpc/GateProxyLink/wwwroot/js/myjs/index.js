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
}

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
function reload() {
    $.get("/gate/device/reloadlist", function (res) {
        load()
    });
}
function reloadServer() {
    $.get("/gate/server/reloadlist", function (res) {
        load()
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
    var data = {
        deviceId: $("#deviceId").text(),
        serverId: $("#serverId").text(),
        ip: $("#ip").val(),
        port: $("#port").val(),
        monitoring: $("#monitoring").val(),
        frequency: $("#frequency").val(),
        storage: $("#storage").val(),
        instruction: $("#instruction").val(),
    }
    $.post("/gate/device/setstorage",
        {
            deviceInfo: JSON.stringify(data)
        }, function (res) {
            alert(res.errmsg)
            load()
        })
};

function btnMonitoring() {
    var data = {
        deviceId: $("#deviceId").text(),
        serverId: $("#serverId").text(),
        ip: $("#ip").val(),
        port: $("#port").val(),
        monitoring: $("#monitoring").val(),
        frequency: $("#frequency").val(),
        instruction: $("#instruction").val(),
        storage: $("#storage").val(),
    }
    $.post("/gate/device/setfrequency",
        {
            deviceInfo: JSON.stringify(data)
        }, function (res) {
            alert(res.errmsg)
            load()
        })
}

function btnSend() {
    var data = {
        deviceId: $("#deviceId").text(),
        serverId: $("#serverId").text(),
        ip: $("#ip").val(),
        port: $("#port").val(),
        monitoring: $("#monitoring").val(),
        frequency: $("#frequency").val(),
        instruction: $("#instruction").val(),
        storage: $("#storage").val(),
    }
    $("#ins").html($("#instruction").val());
    $.post("/gate/device/sendback",
        {
            deviceInfo: JSON.stringify(data)
        }, function (res) {
            $("#text").empty();
            $("#text").append(res.messages[0].Item2)
        })
}

function btnAnalysis() {
    $.post("/gate/device/analysis",
        {
            ins: $("#ins").text(),
            msg: $("#text").text()
        }, function (res) {
            var i = 0;
            $("#d1").empty();
            for (var n in res.datas[0]) {
                $("#d1").append(n + " : " + res.datas[0][n] + "       ");
                i++;
                if (i == 10) {
                    $("#d1").append("\n");
                    i = 0;
                }
            }
            i = 0;
            $("#d2").empty();
            for (var n in res.datas[1]) {
                $("#d2").append(n + " : " + res.datas[1][n] + "       ");
                i++;
                if (i == 10) {
                    $("#d1").append("\n");
                    i = 0;
                }
            }
            i = 0;
            $("#d3").empty();
            for (var n in res.datas[2]) {
                $("#d3").append(n + " : " + res.datas[2][n] + "       ");
                i++;
                if (i == 10) {
                    $("#d1").append("\n");
                    i = 0;
                }
            }
        })

}