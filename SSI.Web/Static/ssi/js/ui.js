$(function () {
    $("[data-toggle='tooltip']").tooltip();
    $('.form_date').datetimepicker({
        language: 'zh-CN',
        format: 'yyyy-mm-dd',
        weekStart: 1,
        todayBtn: 1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        minView: 2,
        maxView: 4,
        forceParse: 0
    });
    $('.form_date_time').datetimepicker({
        language: 'zh-CN',
        format: 'yyyy-mm-dd hh:ii:00',
        weekStart: 1,
        todayBtn: 1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        minView: 0,
        maxView: 4,
        forceParse: 0
    });
    $('.form_select').select2({
        language: "zh-CN",
        placeholder: "请选择..."
    });
    $(".form_file_image").fileinput({
        language: 'zh',
        allowedFileExtensions: ['gif', 'jpg', 'jpeg', 'png'],
        maxFileSize: 1024,
        showUpload: false
    });
    $(".form_file_doc").fileinput({
        language: 'zh',
        allowedFileExtensions: ['txt', 'pdf', 'doc', 'docx', 'xls', 'xlsx', 'ppt', 'pptx'],
        maxFileSize: 2048,
        showUpload: false
    });
    $(":input[required]").closest(".form-group").find("label").append($("<span></span>").css("color", "red").css("font-size", "large").html("*"));
});
$.validator.setDefaults({
    errorElement: "div",
    errorClass: "help-block",
    ignore: "",
    highlight: function (e) {
        $(e).closest(".form-group").removeClass("has-info").addClass("has-error");
    },
    unhighlight: function (e) {
        $(e).closest(".form-group").removeClass("has-error").addClass("has-info");
    }
});
$.validator.addMethod("isCardNo", function (value, element, params) {
    var reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
    return this.optional(element) || (reg.test(value));
}, "身份证号码不正确");
$.validator.addMethod("isMenuTarget", function (value, element, params) {
    var reg = /#|\/(.+)\/(.+)\//;
    return this.optional(element) || (reg.test(value));
}, "格式不正确");
$.validator.addMethod("names", function (value, element, params) {
    var reg = /^(?!_)(?!.*?_$)[a-zA-Z0-9_\u4e00-\u9fa5]+$/;
    return this.optional(element) || (reg.test(value));
}, "名称录入不合法(名称不允许带除'_'外的特殊字符)");
$.validator.addMethod("version", function (value, element, params) {
    var reg = /^(\d\.){1,2}(\d\.){1,2}(\d){1,2}$/;
    return this.optional(element) || (reg.test(value));
}, "版本号录入不合法(版本号不允许带除'.'外的特殊字符)");
$.validator.addMethod("vin", function (value, element, params) {
    var reg = /^[A-Z\d]{17}$/;
    return this.optional(element) || (reg.test(value));
}, "请输入有效车辆VIN(VIN号为17位非特殊字符)");
$.validator.addMethod("chassisno", function (value, element, params) {
    var reg = /^[A-Z\d]{8}$/;
    return this.optional(element) || (reg.test(value));
}, "请输入有效底盘号(底盘号为八位非特殊字符)");
$.validator.addMethod("licenseplate", function (value, element, params) {
    var reg = /^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领军A-Z]{1}[A-Z]{1}[A-Z0-9]{4}[A-Z0-9]{1}$/;
    return this.optional(element) || (reg.test(value));
}, "请输入有效车牌号");

$.validator.addMethod("terminal", function (value, element, params) {
    var reg = /^\d{8}$/;
    return this.optional(element) || (reg.test(value));
}, "请输入有效终端编号(例：00000001)");

$.validator.addMethod("terminalcode", function (value, element, params) {
    var reg = /^\w{8}$/;
    return this.optional(element) || (reg.test(value));
}, "请输入有效终端编码(例：DFZD0001)");

$.validator.addMethod("version", function (value, element, params) {
    var reg = /^\d+(.\d+){2}$/;
    return this.optional(element) || (reg.test(value));
}, "请输入有效版本号(例：1.1.0)");
$.validator.addMethod("phone", function (value, element, params) {
    var reg = /^1(3|4|5|7|8)\d{9}$/;
    return this.optional(element) || (reg.test(value));
}, "请输入有效卡号(例：18000000000)");
$.validator.addMethod("expense", function (value, element, params) {
    var reg = /^(?:[1-9]\d*(?:\.\d{1,2})?|0\.(?:\d[1-9]|[1-9]\d))$/;
    return this.optional(element) || (reg.test(value));
}, "请输入有效资费(例：80.0)");
$.validator.addMethod("puk", function (value, element, params) {
    var reg = /^\d{4}[ ]\d{4}$/;
    return this.optional(element) || (reg.test(value));
}, "请输入有效PUK(例：XXXX XXXX)");
$.validator.addMethod("iccid", function (value, element, params) {
    var reg = /^(8986)\w{1}[ ]\w{5}[ ]\w{5}[ ]\w{5}$/;
    return this.optional(element) || (reg.test(value));
}, "请输入有效ICCID(例：89860 XXXXX XXXXX XXXXX)");
$.authorize = function () {
    if (top.clientData.user.User.F_System_Mark != 1) {
        var menu_id = top.getActivePageId();
        var action = $.grep(top.clientData.user.Actions, function (v, i) {
            return v.F_Menu_Id == menu_id;
        });
        $.each(action, function (i, v) {
            switch (v.F_Type_Mark) {
                case 1:
                    $('[authorize="Menu"]#' + v.F_Code).removeAttr("authorize");
                    break;
                case 2:
                    $('[authorize="Grid"]#' + v.F_Code).removeAttr("authorize");
                    break;
                case 3:
                    $('[authorize="Button"]#' + v.F_Code).removeAttr("authorize");
                    break;
            }
        });
        $('[authorize]').remove();
    }
};
$.reload = function () {
    top.location.reload();
}
$.request = function (name) {
    var search = location.search.slice(1);
    var arr = search.split("&");
    for (var i = 0; i < arr.length; i++) {
        var ar = arr[i].split("=");
        if (ar[0] == name) {
            if (unescape(ar[1]) == 'undefined') {
                return "";
            } else {
                return unescape(ar[1]);
            }
        }
    }
    return "";
}
$.browser = function () {
    var userAgent = navigator.userAgent;
    var isOpera = userAgent.indexOf("Opera") > -1;
    if (isOpera) {
        return "Opera"
    };
    if (userAgent.indexOf("Firefox") > -1) {
        return "FF";
    }
    if (userAgent.indexOf("Chrome") > -1) {
        if (window.navigator.webkitPersistentStorage.toString().indexOf('DeprecatedStorageQuota') > -1) {
            return "Chrome";
        } else {
            return "360";
        }
    }
    if (userAgent.indexOf("Safari") > -1) {
        return "Safari";
    }
    if (userAgent.indexOf("compatible") > -1 && userAgent.indexOf("MSIE") > -1 && !isOpera) {
        return "IE";
    };
};
$.currentWindow = function () {
    return top.$('#tab-content').find('.active').find("iframe").contents();
};
$.parentLayerWindow = function () {
    return top.$("#" + top.clientData.parentLayer).contents();
};
$.jsonWhere = function (data, action) {
    if (action == null) return;
    var reval = [];
    $(data).each(function (i, v) {
        if (action(v)) {
            reval.push(v);
        }
    });
    return reval;
};
$.getParamValues = function (data, name) {
    var result = [];
    $(data).each(function (i, v) {
        result.push(v[name]);
    });
    return result
};
$.modalAlert = function (content, type) {
    var icon = "";
    switch (type) {
        case "success":
            icon = "fa-check-circle";
            break;
        case "error":
            icon = "fa-times-circle";
            break;
        case "warning":
            icon = "fa-exclamation-circle";
            break;
    }
    top.layer.alert(content, {
        icon: icon,
        title: "提示",
        btn: ['确认']
    });
};
$.modalMsg = function (content, type) {
    if (type != undefined) {
        var icon = "";
        switch (type) {
            case "success":
                icon = "fa-check-circle";
                break;
            case "error":
                icon = "fa-times-circle";
                break;
            case "warning":
                icon = "fa-exclamation-circle";
                break;
        }
        top.layer.msg(content, { icon: icon, time: 1500, shade: 0.2 });
    } else {
        top.layer.msg(content, { time: 1500, shade: 0.2 });
    }
};
$.modalConfirm = function (content, callBack) {
    top.layer.confirm(content, {
        icon: "fa-exclamation-circle",
        title: "询问",
        btn: ['确认', '取消']
    }, function (index) {
        callBack && callBack(true);
        top.layer.close(index);
    }, function () {
        callBack && callBack(false)
    });
};
$.modalLoading = function (bool) {
    if (bool) {
        top.layer.load(1, { shade: 0.1 });
    } else {
        top.layer.closeAll('loading');
    }
};
$.modalClose = function () {
    var index = top.layer.getFrameIndex(window.name);
    top.layer.close(index);
};
$.modalOpen = function (params) {
    var defaults = {
        type: 2,
        id: null,
        title: '提示',
        area: ['60%', '90%'],
        maxmin: true,
        fixed: false,
        shade: 0.3,
        content: null,
        success: null
    };
    var options = $.extend(defaults, params);
    if (top.$(window).width() < 768) {
        options.area[0] = '100%';
    }
    top.layer.open(options);
    top.clientData.parentLayer = window.name;
};
$.ajaxSetup({
    dataType: "json",
    error: function (xhr, status, error) {
        $.modalLoading(false);
        $.modalMsg(error, "error");
    },
    beforeSend: function (xhr) {
        $.modalLoading(true);
    },
    complete: function (xhr, status) {
        $.modalLoading(false);
    }
});
$.getToken = function (params) {
    var defaults = {
        data: []
    };
    var options = $.extend(defaults, params);
    if ($('[name=__RequestVerificationToken]').length > 0) {
        options["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
    }
    return options;
};
$.getJson = function (params) {
    params.data = $.getToken(params.data);
    $.ajax(params);
};
$.submitAjax = function (params) {
    var defaults = {
        type: "post"
    };
    var options = $.extend(defaults, params);
    options.data = $.getToken(options.data);
    options.success = function (result, status, xhr) {
        if (result.state == "success") {
            $.modalMsg(result.msg, result.state);
            if (params.hasOwnProperty("success")) {
                params.success(result);
            }
        } else {
            $.modalAlert(result.msg, result.state);
            if (params.hasOwnProperty("failure")) {
                params.failure(result);
            }
        }
    }
    $.ajax(options);
};
$.confirmAjax = function (params) {
    var defaults = {
        prompt: "您确定吗？",
    };
    options = $.extend(defaults, params);
    $.modalConfirm(options.prompt, function (r) {
        if (r) {
            $.submitAjax(options);
        }
    });
};
$.fn.submitForm = function (params) {
    var defaults = {
        type: "post"
    };
    var options = $.extend(defaults, params);
    options.data = $.getToken(options.data);
    options.success = function (result, status, xhr) {
        if (result.state == "success") {
            $.modalMsg(result.msg, result.state);
            if (params.hasOwnProperty("success")) {
                params.success(result);
            }
        } else {
            $.modalAlert(result.msg, result.state);
            if (params.hasOwnProperty("failure")) {
                params.failure(result);
            }
        }
    }
    var $element = $(this);
    $element.validate({
        submitHandler: function (e) {
            $(e).ajaxSubmit(options);
        }
    });
};
$.fn.confirmForm = function (params) {
    var defaults = {
        prompt: "您确定吗？"
    };
    options = $.extend(defaults, params);
    var $element = $(this);
    $.modalConfirm(options.prompt, function (r) {
        if (r) {
            $element.submitForm(options);
        }
    });
};
$.fn.bindSelect = function (params) {
    var defaults = {
        id: "id",
        text: "text",
        async: false,
    };
    var options = $.extend(defaults, params);
    options.data = $.getToken(options.data);
    options.success = function (result) {
        $.each(result, function (i, v) {
            $element.append($("<option></option>").val(v[options.id]).html(v[options.text]));
        });
    }
    var $element = $(this);
    if (options.hasOwnProperty("url")) {
        $.getJson(options);
    }
    if (options.hasOwnProperty("change")) {
        $element.on("change", options.change);
    }
};
$.fn.formSerialize = function (data) {
    if (!$.isEmptyObject(data)) {
        var $element = $(this);
        for (var key in data) {
            var value = data[key];
            var $formField = $element.find('[name="' + key + '"]');
            if ($.type($formField[0]) !== "undefined") {
                var fieldTagName = $formField[0].tagName.toLowerCase();
                if (fieldTagName == "input") {
                    var fieldTypeName = $formField.attr("type");
                    if (fieldTypeName == "radio" || fieldTypeName == "checkbox") {
                        $("input:" + fieldTypeName + "[name='" + key + "'][value='" + value + "']").attr("checked", "checked");
                    } else {
                        $formField.val(value);
                    }
                } else if (fieldTagName == "textarea") {
                    $formField.val(value);
                } else if (fieldTagName == "img") {
                    $formField.attr("src", value);
                } else if (fieldTagName == "select") {
                    $formField.val(value).trigger("change");
                } else {
                    $formField.val(value);
                }
            }
        }
    }
};
$.fn.serializeField = function () {
    var field = [];
    var $element = $(this).bootstrapTable('getVisibleColumns');
    $.each($element, function (i, v) {
        if (!!v.field) {
            field.push({
                ParamTitle: v.title,
                ParamField: v.field
            });
        }
    });
    return JSON.stringify(field);
};
$.fn.serializeQuery = function () {
    var query = [];
    var $element = $(this).find("[operate]");
    $.each($element, function (i, v) {
        if (!!v.value) {
            query.push({
                ParamName: v.name,
                ParamValue: v.value,
                Operation: v.attributes['operate'].value
            });
        }
    });
    return JSON.stringify(query);
};
$.fn.bootstrapTableInit = function (params) {
    var defaults = {
        url: "GetGridList",
        idField: "F_Id",
        toolbar: "#table1_toolbar",
        sortName: "F_Create_Time",
        sortOrder: "Desc",
        search: true,
        showRefresh: true,
        showColumns: true,
        singleSelect: true,
        clickToSelect: true,
        sidePagination: "server",
        pagination: true,
        pageSize: 5,
        pageList: [5, 10, 20, 50, 100],
        paginationPreText: "上页",
        paginationNextText: "下页",
        queryParams: function (params) {
            var query = { query: $("#table1_query").serializeQuery() };
            var options = $.extend(params, query);
            return options;
        }
    };
    var options = $.extend(defaults, params);
    var $element = $(this);
    $element.bootstrapTable(options);
};
$.fn.sidebarMenu = function (params) {
    var defaults = {
        level: 0
    };
    var options = $.extend(defaults, params);
    var $element = $(this);
    if (options.hasOwnProperty("menu")) {
        init($element, options.menu, options.level);
    }
    $element.children(".menu").first().addClass('open');
    function init($element, menu, level) {
        $.each(menu, function (i, v) {
            var $li = $('<li></li>').addClass("menu");
            var $a = $('<a href="#"></a>').addClass("dropdown-toggle");
            var $icon = $('<i></i>').addClass("menu-icon").addClass(v.icon);
            $a.append($icon);
            var $text = $('<span></span>').addClass('menu-text').text(v.text);
            $a.append($text);
            if (v.children && v.children.length > 0) {
                $a.append($('<b></b>').addClass("arrow").addClass("fa").addClass("fa-angle-down"));
                $li.append($a);
                $li.append($('<b></b>').addClass("arrow"));
                var $menus = $('<ul></ul>').addClass('submenu');
                init($menus, v.children, level + 1);
                $li.append($menus);
            } else {
                $a.attr('onclick', 'addTabs({id:\'' + v.id + '\',title: \'' + v.text + '\',url: \'' + v.url + '\',close: true});');
                $li.append($a);
                $li.append($('<b></b>').addClass("arrow"));
            }
            $element.append($li);
        });
    }
};
$.download = function (url, callBack) {
    if (url) {
        var $form = $("<form></form>").css("display", "none").attr("action", url).attr("method", "post");
        $form.appendTo('body');
        $form.trigger("submit").remove();
        callBack && callBack();
    }
};
$.download2 = function (url,field, callBack) {
    if (url) {
        var $form = $("<form></form>").css("display", "none").attr("action", url).attr("method", "post");
        var $field = $("<input/>").attr({
            name: 'field',
            id: 'field',
            value: field
        });
        $field.appendTo($form);
        $form.appendTo('body');
        $form.trigger("submit").remove();
        callBack && callBack();
    }
};
$.upload = function (url, callBack) {
    if (url) {
        var $form = $("<form></form>").css("display", "none").attr("action", url).attr("method", "post");
        var $file = $('<input type="file" />').attr("name", "file");
        $file.on("change", function (e) {
            $form.ajaxSubmit({
                success: function (result) {
                    if (result.state == "success") {
                        $.modalMsg(result.msg, result.state);
                        callBack && callBack();
                    } else {
                        $.modalAlert(result.msg, result.state);
                    }
                    $form.remove();
                }
            });
        }).appendTo($form);
        $form.appendTo('body');
        $file.trigger("click");
    }
};
$.dataFormat = function (date, format) {
    var date = (typeof date !== 'undefined') ? date : new Date();
    var format = (typeof format !== 'undefined') ? format : "yyyy-MM-dd HH:mm:ss";
    var o = {
        "M+": date.getMonth() + 1,
        "q+": Math.floor((date.getMonth() + 3) / 3),
        "d+": date.getDate(),
        "H+": date.getHours(),
        "h+": date.getHours() % 12 == 0 ? 12 : date.getHours() % 12,
        "m+": date.getMinutes(),
        "s+": date.getSeconds(),
        "S": date.getMilliseconds()
    };
    var week = {
        "0": "/u65e5",
        "1": "/u4e00",
        "2": "/u4e8c",
        "3": "/u4e09",
        "4": "/u56db",
        "5": "/u4e94",
        "6": "/u516d"
    };
    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    if (/(E+)/.test(format)) {
        format = format.replace(RegExp.$1, ((RegExp.$1.length > 1) ? (RegExp.$1.length > 2 ? "/u661f/u671f" : "/u5468") : "") + week[date.getDay() + ""]);
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        }
    }
    return format;
};
$.dataAdd = function (date, interval, number) {
    var date = (typeof date !== 'undefined') ? date : new Date();
    var interval = (typeof interval !== 'undefined') ? interval : "d";
    var number = (typeof number !== 'undefined') ? number : 0;
    switch (interval) {
        case "y": {
            date.setFullYear(date.getFullYear() + number);
            return date;
            break;
        }
        case "q": {
            date.setMonth(date.getMonth() + number * 3);
            return date;
            break;
        }
        case "M": {
            date.setMonth(date.getMonth() + number);
            return date;
            break;
        }
        case "w": {
            date.setDate(date.getDate() + number * 7);
            return date;
            break;
        }
        case "d": {
            date.setDate(date.getDate() + number);
            return date;
            break;
        }
        case "H": {
            date.setHours(date.getHours() + number);
            return date;
            break;
        }
        case "m": {
            date.setMinutes(date.getMinutes() + number);
            return date;
            break;
        }
        case "s": {
            date.setSeconds(date.getSeconds() + number);
            return date;
            break;
        }
        default: {
            date.setDate(date.getDate() + number);
            return date;
            break;
        }
    }
};