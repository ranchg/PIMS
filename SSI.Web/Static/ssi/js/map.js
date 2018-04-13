/**
 * 
 * 高德地图javascript调用
 * @author chenwj
 * @Date: 2017/12/8
 *
 **/
//创建地图
var map = null;
var lonlat;//保存窗体的坐标数据

function mapInit() {
        map = new AMap.Map('map', {
        mapStyle: 'amap://styles/normal',//样式URL
        resizeEnable: true,
        zoom: 10
        //center: [116.397428, 39.90923]//默认定位：初始化加载地图时，center属性缺省，地图默认显示用户所在城市范围
        });
       
        AMap.plugin(['AMap.ToolBar', 'AMap.Scale', 'AMap.OverView'],
        function () {
            map.addControl(new AMap.ToolBar());//工具条

            map.addControl(new AMap.Scale());//比例尺

            map.addControl(new AMap.OverView({ isOpen: true }));//鹰眼
        });
}

//地图语言设置
function selectMapLang(lang) {
    var lang_type = lang.value;
    map.setLang(lang_type);//设置语言
}

//构建自定义信息窗体
function createInfoWindow(title, content) {
    var info = document.createElement("div");
    info.className = "info_window";

    //可以通过下面的方式修改自定义窗体的宽高
    //info.style.width = "400px";
    // 定义顶部标题
    var top = document.createElement("div");
    var titleD = document.createElement("div");
    var closeX = document.createElement("img");
    top.className = "info-top";
    titleD.innerHTML = title;
    closeX.src = "/Static/ssi/Images/fullscreen/close2.gif";
    closeX.onclick = closeInfoWindow;

    top.appendChild(titleD);
    top.appendChild(closeX);
    info.appendChild(top);

    // 定义中部内容
    var middle = document.createElement("div");
    middle.className = "info-middle";
    middle.style.backgroundColor = 'white';
    middle.innerHTML = content;
    info.appendChild(middle);

    // 定义底部内容
    var bottom = document.createElement("div");
    bottom.className = "info-bottom";
    bottom.style.position = 'relative';
    bottom.style.top = '0px';
    bottom.style.margin = '0 auto';
    var sharp = document.createElement("img");
    sharp.src = "/Static/ssi/Images/fullscreen/sharp.png";
    bottom.appendChild(sharp);
    info.appendChild(bottom);
    return info;
}

//关闭信息窗体
function closeInfoWindow() {
    map.clearInfoWindow();
}

//添加历史轨迹点
function addTrack(poly, isRemove, isForVL) { //添加轨迹线ploy轨迹坐标,isRemove是否移除所有的覆盖物,isForVL true为VL调用，false为历史轨迹调用
    if (isRemove) {
        map.clearMap(); //删除地图对象上所有覆盖物
    }
    if (poly == "" || poly == null) return;
    var total = poly.split('&');
    var pointsArr = total[0].split('>');
    if (pointsArr.length < 2) return;   //没有轨迹点
    var marker;//存放坐标点数组
    var totalLineArr = [];//存放坐标点数组 totalLineArr为所有点
    var marker1; //声明起点
    var marker2; //声明终点
    for (var j = 0; j < pointsArr.length - 1; j++) {
        var lineArr = new Array(); //正确线段
        var arrCorrect = pointsArr[j].split("|");
        for (i = 0; i < arrCorrect.length - 1; i++) {//正确坐标添加
            if (arrCorrect[i] != "") {
                var t = arrCorrect[i].split(",");
                if (t.length == 3) {
                    lineArr.push([t[0], t[1]]);
                }
            }
        }
        if (j == 0) {
            marker1 = arrCorrect[0].split(",");
            addCommonMarker("起点", marker1[0], marker1[1], "时间：" + marker1[2] + "</br>经度：" + marker1[0] + "</br>纬度：" + marker1[1] + "</br>当前位置： <label id='lblAddress'></label><img src='/Content/Images/fullscreen/loading34.gif' id='imgAddress' style='display:none' /><a href='#' id='aGetAddress' class='GetAddress' onclick='GetAddress();'>获取当前位置</a>", false, "A", "");    
        }
        // 绘制轨迹
        var polyline = new AMap.Polyline({
            map: map,
            path: lineArr,
            strokeColor: "#F00",  //线颜色
            // strokeOpacity: 1,     //线透明度
            strokeWeight: 3,   //线宽
            lineJoin: "round",
            strokeStyle: "solid"  //线样式
        });
        marker2 = arrCorrect[arrCorrect.length - 2].split(",");
    }
    addCommonMarker("终点", marker2[0], marker2[1], "时间：" + marker2[2] + "</br>经度：" + marker2[0] + "</br>纬度：" + marker2[1] + "</br>当前位置： <label id='lblAddress'></label><img src='/Content/Images/fullscreen/loading34.gif' id='imgAddress' style='display:none' /><a href='#' id='aGetAddress' class='GetAddress' onclick='GetAddress();'>获取当前位置</a>", false, "B", "");

    //错误线段
    var Errors = total[1].split('>');
    for (var j = 0; j < Errors.length - 1; j++) {
        var lineErrorArr = new Array(); //错误线段
        var arrError = Errors[j].split("|");
        if (arrError.length > 1) {
            for (i = 0; i < arrError.length - 1; i++) {//错误坐标添加
                if (arrError[i] != "") {
                    var t = arrError[i].split(",");
                    if (t.length == 3) {
                        lineErrorArr.push([t[0], t[1]]);
                        //添加连接点  有问题
                        addCommonMarker("点", t[0], t[1], "时间：" + t[2] + "</br>经度：" + t[0] + "</br>纬度：" + t[1] + "</br>当前位置： <label id='lblAddress'></label><img src='/Images/loading34.gif' id='imgAddress' style='display:none' /><a href='#' id='aGetAddress' class='GetAddress' onclick='GetAddress();'>获取当前位置</a>", false, "C", "");
                    }
                }
            }
        }
        var errline = new AMap.Polyline({
            map: map,
            path: lineErrorArr,
            strokeColor: "#00A",  //线颜色
            // strokeOpacity: 1,     //线透明度
            strokeWeight: 3,      //线宽
            strokeStyle: "dashed"  //线样式(虚线) 
        });
    }

    map.setFitView();
}

//添加marker标记(单点)
function addMarker(poly, type) {
    map.clearMap();
    if (poly == "") {
        return;
    }
    var arr = poly.split("&");
    var alert = arr[0];//在线标识
    var arr2 = arr[1].split(";");//得到各属性值
    //实例化信息窗体
    //var title = arr2[4],
    content = [];
    content.push(arr2[8]);
    addCommonMarker(arr2[4], arr2[0], arr2[1], content, true, "mark_r.png", arr2[4]);
}

//添加一般marker标记 弹框显示内容
function addCommonMarker(vin, lon, lat, contents, isRemove, image, isHasLabel) {
    if (isRemove) {
        map.clearMap();
    }
    var icontemp;
    switch (image) {
        case "A":
            icontemp = '/Static/ssi/Images/fullscreen/A.png';
            break;
        case "B":
            icontemp = '/Static/ssi/Images/fullscreen/B.png';
            break;
        case "C":
            icontemp = '/Static/ssi/Images/fullscreen/exception.png';
            break;
        default:
            icontemp = '/Static/ssi/Images/fullscreen/mark_r.png';
            break;
    }
    var marker = new AMap.Marker({
        icon: icontemp,
        position: [lon, lat],
        map: map
    });

    if (isHasLabel != "") {//isHasLabel不为空时,为mark添加label
        marker.setLabel({
            offset: new AMap.Pixel(20, 20),//修改label相对于maker的位置
            content: vin
        });
    }
    map.setFitView();// 添加事件监听, 使地图自适应显示到合适的范围

    //实例化信息窗体
    var title = vin,
    content = [];
    content.push(contents);
    var infoWindow = new AMap.InfoWindow({
        isCustom: true,  //使用自定义窗体
        content: createInfoWindow(title, content.join("<br/>")),
        offset: new AMap.Pixel(16, -45)
    });

    //鼠标点击marker弹出自定义的信息窗体
    AMap.event.addListener(marker, 'click', function () {
        infoWindow.open(map, marker.getPosition());
        lonlat = [lon, lat];
    });
}

function GetAddress() {
    $("#lblAddress").html("正在获取");
    $("#imgAddress").css("display", "inline");
    $("#aGetAddress").css("display", "none");
    //alert(lonlat);
    $.post("/VehicleMonitor/HistoryTrack/GetAddress", { lon: lonlat[0], lat: lonlat[1], timestamp: (new Date()).getTime() }, function (data, textStatus) {
        if (textStatus == "success") {
            $("#lblAddress").html(data);
            $("#imgAddress").css("display", "none");
        }
    },"text");

}

//加载海量点 flag: checked　是否全选选中
function loadAllPoints(flag) {
    if (flag == true) {
        AMapUI.load(['ui/misc/PointSimplifier', 'lib/$'], function (PointSimplifier, $) {

            if (!PointSimplifier.supportCanvas) {
                alert('当前环境不支持 Canvas！');
                return;
            }

            var pointSimplifierIns = new PointSimplifier({
                map: map, //所属的地图实例

                getPosition: function (item) {

                    if (!item) {
                        return null;
                    }

                    var parts = item.split(',');

                    //返回经纬度
                    return [parseFloat(parts[0]), parseFloat(parts[1])];
                },
                getHoverTitle: function (dataItem, idx) {
                    return idx + ': ' + dataItem;
                },
                renderOptions: {
                    //点的样式
                    pointStyle: {
                        content: PointSimplifier.Render.Canvas.getImageContent('/Static/ssi/Images/fullscreen/zero_position.png',
                                function onload() {
                                    //图片加载成功，重新绘制一次
                                    pointSimplifierIns.renderLater();
                                },
                                 function onerror(e) {
                                     alert('图片加载失败！');
                                 }),
                        //offset: ['-50%', '-100%'],
                        width: 30,
                        height: 30,
                        fillStyle: 'blue'
                    },
                    //鼠标hover时的title信息
                    hoverTitleStyle: {
                        position: 'top'
                    }
                }
            });

            window.pointSimplifierIns = pointSimplifierIns;

            $('<div id="loadingTip">加载数据，请稍候...</div>').appendTo(document.body);
            $.ajax({
                //要用post方式
                type: "post",
                //方法所在页面和方法名
                url: "/VehicleMonitor/FullScreen/PointSimplifier",
                dataType: "text",
                success: function (csv) {
                    var data = csv.split('|');
                    pointSimplifierIns.setData(data);
                    $('#loadingTip').remove();
                },
                error: function (err) {
                    alert(err);
                }
            });

            pointSimplifierIns.on('pointClick', function (e, record) {
                //console.log(e.type, record);
                alert(record.index);
            });
        });
    } else {
        alert(flag);
        pointSimplifierIns.setData([])
    }
    
}


//type :monitor  single  multiple
function AddPoints(chassisnos,type) {
    if (chassisnos != "") {
        //获取点
        $.post("/VehicleMonitor/FullScreen/MultiplePoints",
            { data: chassisnos, timestamp: (new Date()).getTime() }
            , function (data, textStatus) {
                if (textStatus == "success")
                    addMultipleMarkers(data, type);
            }, "text");

    } else {
        map.clearMap();
    }
}

//添加多个marker标记 type：singel(单点) multiple(多点)
function addMultipleMarkers(data,type)
{
    map.clearMap();
    clearInterval(time1); //清空暂停定时器
    if (data == "") {
        return;
    }
    var record = data.split(",");//获取记录的条数
    for (var i = 0; i < record.length; i++) {
        var arr = record[i].split("&");
        var isonline = arr[0];//在线标识
        var arr2 = arr[1].split(";");//得到各属性值
        content = [];
        content.push(arr2[4]);//弹框内容
        if (type == "monitor") {//实时监控
            markerMove(arr2[2], arr2[3], arr2[0], arr2[1], content);
        } else {
            addCommonMarker(arr2[2], arr2[0], arr2[1], content, false, "mark_r.png", arr2[2]);
        }
    }
}

//单点跟踪
var moveLineArr =[]; //存放线的数组 
var time1;//定时器
var monitorMarker;//移动点
//实时取点坐标 licenseplate:车牌(VIN)  chassisno:底盘号  lon:第一个点经度  lat：第一个点纬度
function markerMove(licenseplate, chassisno, lon, lat, contents) {
    moveLineArr = [];//每次单车跟踪时，先清点数组
    moveLineArr.push([lon, lat]);//将添加点加入数组中
    //先创建一个点
    monitorMarker = new AMap.Marker({
        icon: '/Static/ssi/Images/fullscreen/mark_r.png',
        position: [lon, lat],
        map: map
    });
    map.setFitView();// 添加事件监听, 使地图自适应显示到合适的范围

    //实例化信息窗体
    var title = licenseplate,
    content = [];
    content.push(contents);
    var infoWindow = new AMap.InfoWindow({
        isCustom: true,  //使用自定义窗体
        content: createInfoWindow(title, content.join("<br/>")),
        offset: new AMap.Pixel(16, -45)
    });

    //鼠标点击marker弹出自定义的信息窗体
    AMap.event.addListener(monitorMarker, 'click', function () {
        infoWindow.open(map, monitorMarker.getPosition());
        lonlat = [lon, lat];
    });


    time1 = setInterval(function () {//定时器，定时去数据库取当前点,时间间隔30s
        $.post("/VehicleMonitor/FullScreen/MultiplePoints",
           { data: chassisno, timestamp: (new Date()).getTime() }
           , function (data, textStatus) {
               if (textStatus == 'success')
                   markRealLine(licenseplate,data, moveLineArr);
                   
        });
    }, 1000 * 30);
}

function markRealLine(licenseplate, poly, moveLineArr) {
    if (poly == "") {
        return;
    }
    var arr = poly.split("&");
    var isonline = arr[0];//在线标识
    var arr2 = arr[1].split(";");//得到各属性值
    content = [];
    content.push(arr2[4]);
    moveLineArr.push([arr2[0], arr2[1]]);//将新获取的坐标点加入数组中
    if (moveLineArr.length > 2) {
        moveLineArr.splice(0, 1);//删除最开始的一个元素，始终保留2个点坐标
    }
    //alert(moveLineArr.join(','))
    monitorMarker.moveTo([arr2[0], arr2[1]], 500);//移动点
    var polyline = new AMap.Polyline({
        map: map,
        path: moveLineArr,          //设置线覆盖物路径
        strokeColor: "#F00",  //线颜色
        strokeWeight: 3,   //线宽
        lineJoin: "round",
        strokeStyle: "solid"  //线样式
    });

    //实例化信息窗体
    var title = licenseplate;
    var infoWindow = new AMap.InfoWindow({
        isCustom: true,  //使用自定义窗体
        content: createInfoWindow(title, content.join("<br/>")),
        offset: new AMap.Pixel(16, -45)
    });
    //鼠标点击marker弹出自定义的信息窗体
    AMap.event.addListener(monitorMarker, 'click', function () {
        infoWindow.open(map, monitorMarker.getPosition());
        lonlat = [arr2[0], arr2[1]];
    });
    //map.setFitView();// 使地图自适应显示到合适的范围(报错)
    map.setCenter([arr2[0], arr2[1]]);
}


