
function StopPropagation(e) {

    e = e || window.event;
    if (e.preventDefault) {
        e.preventDefault();
        e.stopPropagation();
    } else {
        e.returnValue = false;
        e.cancelBubble = true;
    }

}


var SSIcityPicker = function (options) {
    template_html = '<div class="panel panel-primary ssi-city-picker-panel" id="ssi-city-picker-panel">';
    template_html += '<ul class="nav nav-pills">';
    template_html += '<li role="presentation" class="active" id="panel-title-province">';
    template_html += '<a href="#province" >省份</a></li>';
    template_html += '<li role="presentation"  id="panel-title-city">';
    template_html += '<a href="#city" >市区</a></li>';
    template_html += '<li role="presentation"  id="panel-title-area">';
    template_html += '<a href="#area" >区县</a></li></ul>';
    template_html += '<div class="tab-content">';
    template_html += '<div role="tabpanel" class="tab-pane active" id="panel-content-province">ssss</div>';
    template_html += '<div role="tabpanel" class="tab-pane" id="panel-content-city">sssss</div>';
    template_html += '<div role="tabpanel" class="tab-pane" id="panel-content-area">sssss</div>';
    template_html += '</div></div>';
    this.template = $(template_html);
    this.province_title = $("#panel-title-province", this.template);
    this.province_wrap = $("#panel-content-province", this.template);
    this.city_wrap = $("#panel-content-city", this.template);
    this.city_title = $("#panel-title-city", this.template);
    this.area_wrap = $("#panel-content-area", this.template);
    this.area_title = $("#panel-title-area", this.template);

    this.settings = {
        "data": options.data,
        "target": $(options.target)
    }
}

SSIcityPicker.prototype = {
    init: function () {
        var that = this;
        $('body').click(function (event) {
            that.template.remove();

        });

        that.settings.target.attr('readonly', true);

        that.targetEvent();
    },

    buildAreaPicker: function () {

        var that = this;
        that.resetContent();
        that.provinceEvent();
        that.cityEvent();
        that.areaEvent();
        that.provinceTitleEvent();
        that.cityTitleEvent();
        that.areaTitleEvent();
    },

    buildCityPicker: function () {
        var that = this;
        that.resetContent();
        that.provinceEvent();
        that.cityEvent();
        that.provinceTitleEvent();
        that.cityTitleEvent();
    },

    buildProvinceTpl: function () {//省级div显示
        var that = this;
        var province = that.settings.data.province;
        var province_html = '';
        for (var i = 0, len = province.length; i < len; i++) {
            province_html += '<span class="panel-content-label" data-id="' + province[i]['id'] + '" data-name="' + province[i]['name'] + '">' + province[i]['name'] + '</span>'
        }

        that.province_wrap.html(province_html);
    },
    buildCityTpl: function (cur_province) {//省级下的市级div显示（选中的省下的市div显示）
        var that = this;
        var pid = cur_province.data('id');
        var poi = cur_province.position();
        var province = that.settings.data.province;
        var city;
        var city_html = '';
        for (var i = 0, plen = province.length; i < plen; i++) {
            if (province[i]['id'] == parseInt(pid)) {
                city = province[i]['city'];
                break;
            }
        }
        for (var j = 0, clen = city.length; j < clen; j++) {
            city_html += '<span class="panel-content-label" data-id="' + city[j]['id'] + '" data-name="' + city[j]['name'] + '" data-province="' + province[i]['name'] + '">' + city[j]['name'] + '</span>'
        }
        that.city_wrap.html(city_html);
    },
    buildAreaTpl: function (cur_city) {//选中市下的区域div显示
        var that = this;
        var area_html = '';
        var pid = cur_city.data('id');
        var poi = cur_city.position();
        var province = that.settings.data.province;
        for (var i = 0; i < province.length; i++) {
            for (var j = 0; j < province[i]["city"].length; j++) {
                if (province[i]["city"][j]["id"] == pid) {
                    for (var k = 0; k < province[i]["city"][j]["area"].length; k++) {
                        area_html += '<span class="panel-content-label" data-fid="' + province[i]["city"][j]["area"][k]["fid"] + '" data-area="' + province[i]["city"][j]["area"][k]["name"] + '" data-city="' + province[i]["city"][j]["name"] + '" data-province="' + province[i]["name"] + '">' + province[i]["city"][j]["area"][k]["name"] + '</span>';
                    }
                }
            }
        }
        that.area_wrap.html(area_html);
    },
    provinceEvent: function () {
        var that = this;
        $('.panel-content-label', that.province_wrap).click(function (event) {
            //event.stopPropagation();
            StopPropagation(event);
            var _this = $(this);
            that.buildCityTpl(_this);
            that.province_title.removeClass("active");
            that.province_wrap.removeClass("active");
            that.city_title.addClass("active");
            that.city_wrap.addClass("active");
            return false;
        })
    },
    cityEvent: function () {
        var that = this;
        that.city_wrap.on('click', '.panel-content-label', function () {
            StopPropagation(event);
            var _this = $(this);
            that.buildAreaTpl(_this);
            that.province_title.removeClass("active");
            that.province_wrap.removeClass("active");
            that.city_title.removeClass("active");
            that.city_wrap.removeClass("active");
            that.area_title.addClass("active");
            that.area_wrap.addClass("active");
            return false;
        });
    },
    areaEvent: function () {
        var that = this;
        that.area_wrap.on('click', '.panel-content-label', function () {
            var _this = $(this);
            //var city = _this.data('province');
            var area = ""; 
            if (_this.data('city') == "市辖区" || _this.data('city') == "县" || _this.data('city')=="市") 
                area = _this.data('province') + _this.data('area');
             else 
                area = _this.data('province') + _this.data('city') + _this.data('area');
            //取消红线提示
            if ($("#City").siblings().length > 1)
                $("#City").siblings().remove("div");
            if ($("#City").attr("style") != undefined || $("#City").attr("style") != "")
                $("#City").attr("style", "");
            if ($("#City").parent().siblings().attr("style") != undefined || $("#City").parent().siblings().attr("style") != "")
                $("#City").parent().siblings().attr("style", "");
            //
            var fid = _this.data("fid");
            that.settings.target.val(area);
            $("#F_Area_Id").val(fid);
            
        });
    },
    provinceTitleEvent: function () {//给province的li菜单栏赋值css
        var that = this;
        that.province_title.on('click', 'a', function (event) {
            //event.preventDefault();
            //event.stopPropagation();  
            StopPropagation(event);
            that.province_title.addClass("active");
            that.province_wrap.addClass("active");
            that.city_title.removeClass("active");
            that.city_wrap.removeClass("active");
            that.area_title.removeClass("active");
            that.area_wrap.removeClass("active");
            return false;
        })
    },
    cityTitleEvent: function () {//给city的li菜单栏赋值css
        var that = this;
        that.city_title.on('click', 'a', function (event) {
            //event.preventDefault();
            //event.stopPropagation();
            StopPropagation(event);

            that.province_title.removeClass("active");
            that.province_wrap.removeClass("active");
            that.city_title.addClass("active");
            that.city_wrap.addClass("active");
            that.area_title.removeClass("active");
            that.area_wrap.removeClass("active");
            return false;
        })
    },
    areaTitleEvent: function () {//给area的li菜单栏赋值css
        var that = this;
        that.area_title.on('click', 'a', function (event) {
            //event.preventDefault();
            //event.stopPropagation();
            StopPropagation(event);
            that.province_title.removeClass("active");
            that.province_wrap.removeClass("active");
            that.city_title.removeClass("active");
            that.city_wrap.removeClass("active");
            that.area_title.addClass("active");
            that.area_wrap.addClass("active");
            return false;
        })
    },
    resetContent: function () {//重置样式
        var that = this;
        that.province_wrap.html('');
        that.city_wrap.html('');
        that.area_wrap.html('');
        that.buildProvinceTpl();
        that.province_title.addClass("active");
        that.province_wrap.addClass("active");
        that.city_title.removeClass("active");
        that.city_wrap.removeClass("active");
        that.area_title.removeClass("active");
        that.area_wrap.removeClass("active");
    },
    targetEvent: function () {
        var that = this;

        that.settings.target.click(function (event) {
            StopPropagation(event);
            /* Act on the event */
            var _this = $(this);
            that.buildCityPicker();
            that.buildAreaPicker();
            var offset = _this.offset();
            var top = offset.top + _this.outerHeight() + 5;

            that.template.css({
                'left': offset.left,
                'top': top
            });
            $('body').append(that.template);
            return false;
        })
    }

}