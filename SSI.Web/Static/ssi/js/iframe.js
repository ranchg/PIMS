var context = (function () {
    var options = {
        fadeSpeed: 100,
        filter: function ($obj) {
        },
        above: 'auto',
        left: 'auto',
        preventDoubleContext: true,
        compress: false
    };
    function initialize(opts) {
        options = $.extend({}, options, opts);
        $(document).on('click', function () {
            $('.dropdown-context').fadeOut(options.fadeSpeed, function () {
                $('.dropdown-context').css({display: ''}).find('.drop-left').removeClass('drop-left');
            });
        });
        if (options.preventDoubleContext) {
            $(document).on('contextmenu', '.dropdown-context', function (e) {
                e.preventDefault();
            });
        }
        $(document).on('mouseenter', '.dropdown-submenu', function () {
            var $sub = $(this).find('.dropdown-context-sub:first'),
                subWidth = $sub.width(),
                subLeft = $sub.offset().left,
                collision = (subWidth + subLeft) > window.innerWidth;
            if (collision) {
                $sub.addClass('drop-left');
            }
        });
    }
    function updateOptions(opts) {
        options = $.extend({}, options, opts);
    }
    function buildMenu(data, id, subMenu) {
        var subClass = (subMenu) ? ' dropdown-context-sub' : '',
            compressed = options.compress ? ' compressed-context' : '',
            $menu = $('<ul class="dropdown-menu dropdown-context' + subClass + compressed + '" id="dropdown-' + id + '"></ul>');
        return buildMenuItems($menu, data, id, subMenu);
    }
    function buildMenuItems($menu, data, id, subMenu, addDynamicTag) {
        var linkTarget = '';
        for (var i = 0; i < data.length; i++) {
            if (typeof data[i].divider !== 'undefined') {
                var divider = '<li class="divider';
                divider += (addDynamicTag) ? ' dynamic-menu-item' : '';
                divider += '"></li>';
                $menu.append(divider);
            } else if (typeof data[i].header !== 'undefined') {
                var header = '<li class="nav-header';
                header += (addDynamicTag) ? ' dynamic-menu-item' : '';
                header += '">' + data[i].header + '</li>';
                $menu.append(header);
            } else if (typeof data[i].menu_item_src !== 'undefined') {
                var funcName;
                if (typeof data[i].menu_item_src === 'function') {
                    if (data[i].menu_item_src.name === "") {
                        for (var globalVar in window) {
                            if (data[i].menu_item_src == window[globalVar]) {
                                funcName = globalVar;
                                break;
                            }
                        }
                    } else {
                        funcName = data[i].menu_item_src.name;
                    }
                } else {
                    funcName = data[i].menu_item_src;
                }
                $menu.append('<li class="dynamic-menu-src" data-src="' + funcName + '"></li>');
            } else {
                if (typeof data[i].href == 'undefined') {
                    data[i].href = '#';
                }
                if (typeof data[i].target !== 'undefined') {
                    linkTarget = ' target="' + data[i].target + '"';
                }
                if (typeof data[i].subMenu !== 'undefined') {
                    var sub_menu = '<li class="dropdown-submenu';
                    sub_menu += (addDynamicTag) ? ' dynamic-menu-item' : '';
                    sub_menu += '"><a tabindex="-1" href="' + data[i].href + '">' + data[i].text + '</a></li>'
                    $sub = (sub_menu);
                } else {
                    var element = '<li';
                    element += (addDynamicTag) ? ' class="dynamic-menu-item"' : '';
                    element += '><a tabindex="-1" href="' + data[i].href + '"' + linkTarget + '>';
                    if (typeof data[i].icon !== 'undefined')
                        element += '<span class="glyphicon ' + data[i].icon + '"></span> ';
                    element += data[i].text + '</a></li>';
                    $sub = $(element);
                }
                if (typeof data[i].action !== 'undefined') {
                    $action = data[i].action;
                    $sub
                        .find('a')
                        .addClass('context-event')
                        .on('click', createCallback($action));
                }
                $menu.append($sub);
                if (typeof data[i].subMenu != 'undefined') {
                    var subMenuData = buildMenu(data[i].subMenu, id, true);
                    $menu.find('li:last').append(subMenuData);
                }
            }
            if (typeof options.filter == 'function') {
                options.filter($menu.find('li:last'));
            }
        }
        return $menu;
    }
    function addContext(selector, data) {
        if (typeof data.id !== 'undefined' && typeof data.data !== 'undefined') {
            var id = data.id;
            $menu = $('body').find('#dropdown-' + id)[0];
            if (typeof $menu === 'undefined') {
                $menu = buildMenu(data.data, id);
                $('body').append($menu);
            }
        } else {
            var d = new Date(),
                id = d.getTime(),
                $menu = buildMenu(data, id);
            $('body').append($menu);
        }
        $(selector).on('contextmenu', function (e) {
            e.preventDefault();
            e.stopPropagation();
            rightClickEvent = e;
            currentContextSelector = $(this);
            $('.dropdown-context:not(.dropdown-context-sub)').hide();
            $dd = $('#dropdown-' + id);
            $dd.find('.dynamic-menu-item').remove();
            $dd.find('.dynamic-menu-src').each(function (idx, element) {
                var menuItems = window[$(element).data('src')]($(selector));
                $parentMenu = $(element).closest('.dropdown-menu.dropdown-context');
                $parentMenu = buildMenuItems($parentMenu, menuItems, id, undefined, true);
            });
            if (typeof options.above == 'boolean' && options.above) {
                $dd.addClass('dropdown-context-up').css({
                    top: e.pageY - 20 - $('#dropdown-' + id).height(),
                    left: e.pageX - 13
                }).fadeIn(options.fadeSpeed);
            } else if (typeof options.above == 'string' && options.above == 'auto') {
                $dd.removeClass('dropdown-context-up');
                var autoH = $dd.height() + 12;
                if ((e.pageY + autoH) > $('html').height()) {
                    $dd.addClass('dropdown-context-up').css({
                        top: e.pageY - 20 - autoH,
                        left: e.pageX - 13
                    }).fadeIn(options.fadeSpeed);
                } else {
                    $dd.css({
                        top: e.pageY + 10,
                        left: e.pageX - 13
                    }).fadeIn(options.fadeSpeed);
                }
            }
            if (typeof options.left == 'boolean' && options.left) {
                $dd.addClass('dropdown-context-left').css({
                    left: e.pageX - $dd.width()
                }).fadeIn(options.fadeSpeed);
            } else if (typeof options.left == 'string' && options.left == 'auto') {
                $dd.removeClass('dropdown-context-left');
                var autoL = $dd.width() - 12;
                if ((e.pageX + autoL) > $('html').width()) {
                    $dd.addClass('dropdown-context-left').css({
                        left: e.pageX - $dd.width() + 13
                    });
                }
            }
        });
    }
    function destroyContext(selector) {
        $(document).off('contextmenu', selector).off('click', '.context-event');
    }
    return {
        init: initialize,
        settings: updateOptions,
        attach: addContext,
        destroy: destroyContext
    };
})();

var currentContextSelector = undefined;
var rightClickEvent = undefined;
var isFullScreen = false;
var pageIdField = "data-pageId";

function createCallback (func) {
    return function (event) {
        func(event, currentContextSelector,rightClickEvent)
    };
};
function requestFullScreen () {
    var de = document.documentElement;
    if (de.requestFullscreen) {
        de.requestFullscreen();
    } else if (de.mozRequestFullScreen) {
        de.mozRequestFullScreen();
    } else if (de.webkitRequestFullScreen) {
        de.webkitRequestFullScreen();
    }
    else {
        alert("该浏览器不支持全屏！");
    }
};
function exitFull () {
    var exitMethod = document.exitFullscreen ||
        document.mozCancelFullScreen ||
        document.webkitExitFullscreen ||
        document.webkitExitFullscreen; 
    if (exitMethod) {
        exitMethod.call(document);
    }
    else if (typeof window.ActiveXObject !== "undefined") {
        var wscript = new ActiveXObject("WScript.Shell");
        if (wscript !== null) {
            wscript.SendKeys("{F11}");
        }
    }
};
function handleFullScreen () {
    if (isFullScreen) {
        exitFull();
        isFullScreen = false;
    } else {
        requestFullScreen();
        isFullScreen = true;
    }
};
function getViewPort () {
    var e = window,
        a = 'inner';
    if (!('innerWidth' in window)) {
        a = 'client';
        e = document.documentElement || document.body;
    }
    return {
        width: e[a + 'Width'],
        height: e[a + 'Height']
    };
};
function fixIframeCotent() {
    var ht = $(window).height();
    var $footer = $(".main-footer");
    var $header = $(".main-header");
    var $tabs = $(".content-tabs");
    var height = getViewPort().height - $footer.outerHeight() - $header.outerHeight();
    if ($tabs.is(":visible")) {
        height = height - $tabs.outerHeight();
    }
    $(".tab_iframe").css({
        height: height,
        width: "100%"
    });
};
function getPageId(element) {
    if (element instanceof jQuery) {
        return element.attr(pageIdField);
    } else {
        return $(element).attr(pageIdField);
    }
};
function findTabTitle(pageId) {
    var $ele = null;
    $(".page-tabs-content").find("a.menu_tab").each(function () {
        var $a = $(this);
        if ($a.attr(pageIdField) == pageId) {
            $ele = $a;
            return false;
        }
    });
    return $ele;
};
function findTabPanel(pageId) {
    var $ele = null;
    $("#tab-content").find("div.tab-pane").each(function () {
        var $div = $(this);
        if ($div.attr(pageIdField) == pageId) {
            $ele = $div;
            return false;
        }
    });
    return $ele;
};
function findIframeById(pageId) {
    return findTabPanel(pageId).children("iframe");
};
function getActivePageId() {
    var $a = $('.page-tabs-content').find('.active');
    return getPageId($a);
};
function canRemoveTab(pageId) {
    return findTabTitle(pageId).find('.page_tab_close').length > 0;
};
function closeTabByPageId(pageId) {
    var $title = findTabTitle(pageId);
    var $tabPanel = findTabPanel(pageId);
    if ($title.hasClass("active")) {
        var $nextTitle = $title.next();
        var activePageId;
        if ($nextTitle.length > 0) {
            activePageId = getPageId($nextTitle);
        } else {
            activePageId = getPageId($title.prev());
        }
        setTimeout(function () {
            activeTabByPageId(activePageId);
        }, 100);
    } else {

    }
    $title.remove();
    $tabPanel.remove();
};
function closeTabOnly(pageId) {
    var $title = findTabTitle(pageId);
    var $tabPanel = findTabPanel(pageId);
    $title.remove();
    $tabPanel.remove();
};
function closeCurrentTab () {
    var pageId = getActivePageId();
    if (canRemoveTab(pageId)) {
        closeTabByPageId(pageId);
    }
};
function refreshTabById(pageId) {
    var $iframe = findIframeById(pageId);
    var url = $iframe.attr('src');
    $iframe[0].contentWindow.location.reload(true);
    $.modalLoading(true);
};
function refreshTab () {
    var pageId = getActivePageId();
    refreshTabById(pageId);
};
function getTabUrlById(pageId) {
    var $iframe = findIframeById(pageId);
    return $iframe[0].contentWindow.location.href;
};
function getTabUrl(element) {
    var pageId = getPageId(element);
    getTabUrlById(pageId);
};
function editTabTitle(pageId, title) {
    var $title = findTabTitle(pageId);
    var $span = $title.children("span.page_tab_title");
    $span.text(title);
};
function calSumWidth (element) {
    var width = 0;
    $(element).each(function () {
        width += $(this).outerWidth(true);
    });
    return width;
};
function activeTabByPageId(pageId) {
    $(".menu_tab").removeClass("active");
    $("#tab-content").find(".active").removeClass("active");
    var $title = findTabTitle(pageId).addClass('active');
    findTabPanel(pageId).addClass("active");
    scrollToTab($title[0]);
};
function closeOtherTabs (isAll) {
    if (isAll) {
        $('.page-tabs-content').children("[" + pageIdField + "]").find('.page_tab_close').parents('a').each(function () {
            var $a = $(this);
            var pageId = getPageId($a);
            closeTabOnly(pageId);
        });
        var firstChild = $(".page-tabs-content").children().eq(0);
        if (firstChild) {
            activeTabByPageId(getPageId(firstChild));
        }
    } else {
        $('.page-tabs-content').children("[" + pageIdField + "]").find('.page_tab_close').parents('a').not(".active").each(function () {
            var $a = $(this);
            var pageId = getPageId($a);
            closeTabOnly(pageId);
        });
    }
};
function scrollToTab (element) {
    var marginLeftVal = calSumWidth($(element).prevAll()),
        marginRightVal = calSumWidth($(element).nextAll());
    var tabOuterWidth = calSumWidth($(".content-tabs").children().not(".menuTabs"));
    var visibleWidth = $(".content-tabs").outerWidth(true) - tabOuterWidth;
    var scrollVal = 0;
    if ($(".page-tabs-content").outerWidth() < visibleWidth) {
        scrollVal = 0;
    } else if (marginRightVal <= (visibleWidth - $(element).outerWidth(true) - $(element).next().outerWidth(true))) {
        if ((visibleWidth - $(element).next().outerWidth(true)) > marginRightVal) {
            scrollVal = marginLeftVal;
            var tabElement = element;
            while ((scrollVal - $(tabElement).outerWidth()) > ($(".page-tabs-content").outerWidth() - visibleWidth)) {
                scrollVal -= $(tabElement).prev().outerWidth();
                tabElement = $(tabElement).prev();
            }
        }
    } else if (marginLeftVal > (visibleWidth - $(element).outerWidth(true) - $(element).prev().outerWidth(true))) {
        scrollVal = marginLeftVal - $(element).prev().outerWidth(true);
    }
    $('.page-tabs-content').animate({
        marginLeft: 0 - scrollVal + 'px'
    }, "fast");
};
function scrollTabLeft () {
    var marginLeftVal = Math.abs(parseInt($('.page-tabs-content').css('margin-left')));
    var tabOuterWidth = calSumWidth($(".content-tabs").children().not(".menuTabs"));
    var visibleWidth = $(".content-tabs").outerWidth(true) - tabOuterWidth;
    var scrollVal = 0;
    if ($(".page-tabs-content").width() < visibleWidth) {
        return false;
    } else {
        var tabElement = $(".menu_tab:first");
        var offsetVal = 0;
        while ((offsetVal + $(tabElement).outerWidth(true)) <= marginLeftVal) {
            offsetVal += $(tabElement).outerWidth(true);
            tabElement = $(tabElement).next();
        }
        offsetVal = 0;
        if (calSumWidth($(tabElement).prevAll()) > visibleWidth) {
            while ((offsetVal + $(tabElement).outerWidth(true)) < (visibleWidth) && tabElement.length > 0) {
                offsetVal += $(tabElement).outerWidth(true);
                tabElement = $(tabElement).prev();
            }
            scrollVal = calSumWidth($(tabElement).prevAll());
        }
    }
    $('.page-tabs-content').animate({
        marginLeft: 0 - scrollVal + 'px'
    }, "fast");
};
function scrollTabRight () {
    var marginLeftVal = Math.abs(parseInt($('.page-tabs-content').css('margin-left')));
    var tabOuterWidth = calSumWidth($(".content-tabs").children().not(".menuTabs"));
    var visibleWidth = $(".content-tabs").outerWidth(true) - tabOuterWidth;
    var scrollVal = 0;
    if ($(".page-tabs-content").width() < visibleWidth) {
        return false;
    } else {
        var tabElement = $(".menu_tab:first");
        var offsetVal = 0;
        while ((offsetVal + $(tabElement).outerWidth(true)) <= marginLeftVal) {
            offsetVal += $(tabElement).outerWidth(true);
            tabElement = $(tabElement).next();
        }
        offsetVal = 0;
        while ((offsetVal + $(tabElement).outerWidth(true)) < (visibleWidth) && tabElement.length > 0) {
            offsetVal += $(tabElement).outerWidth(true);
            tabElement = $(tabElement).next();
        }
        scrollVal = calSumWidth($(tabElement).prevAll());
        if (scrollVal > 0) {
            $('.page-tabs-content').animate({
                marginLeft: 0 - scrollVal + 'px'
            }, "fast");
        }
    }
};
function addTabs (options) {
    var defaults = {
        title: "新页面"
    };
    var options = $.extend(defaults, options);
    if (!isNaN(options.id)) {
        var pageId = options.id;
        if (findTabPanel(pageId) == null) {
            $.modalLoading(true);
            var $title = $('<a href="javascript:void(0);"></a>').addClass("menu_tab").attr(pageIdField, pageId);
            var $text = $("<span class='page_tab_title'></span>").text(options.title).appendTo($title);
            if (options.close) {
                var $i = $("<i class='glyphicon glyphicon-remove page_tab_close' style='cursor:pointer;margin-left:5px' onclick='closeTab(this);'></i>").attr(pageIdField, pageId).appendTo($title);
            }
            $(".page-tabs-content").append($title);
            var $tabPanel = $('<div role="tabpanel" class="tab-pane"></div>').attr(pageIdField, pageId);
            if (options.content) {
                $tabPanel.append(options.content);
            } else {
                var $iframe = $("<iframe></iframe>").addClass("tab_iframe").css("width", "100%").attr("frameborder", "no").attr("id", "iframe_" + pageId).attr("src", options.url).attr(pageIdField, pageId).attr("menuId", pageId);
                $iframe.on('load', function () {
                    fixIframeCotent();
                    $.modalLoading(false);
                });
                $tabPanel.append($iframe);
            }
            $("#tab-content").append($tabPanel);
        }
        activeTabByPageId(pageId);
    } else {
        alert("参数错误");
    }
};
function closeTab (item) {
    var pageId = getPageId(item);
    closeTabByPageId(pageId);
};
$(function () {
    var $tabs = $(".menuTabs");
    $tabs.on("click", ".menu_tab", function () {
        var pageId = getPageId(this);
        activeTabByPageId(pageId);
    });
    $tabs.on("dblclick", ".menu_tab", function () {
        var pageId = getPageId(this);
        refreshTabById(pageId);
    });
    function findTabElement(target) {
        var $ele = $(target);
        if (!$ele.is("a")) {
            $ele = $ele.parents("a.menu_tab");
        }
        return $ele;
    }
    context.init({
        preventDoubleContext: false,
        compress: true
    });
    context.attach('.page-tabs-content', [
        {
            text: '刷新',
            action: function (e, $selector, rightClickEvent) {
                var pageId = getPageId(findTabElement(rightClickEvent.target));
                refreshTabById(pageId);
            }
        },
        {
            text: "在新窗口打开",
            action: function (e, $selector, rightClickEvent) {
                var pageId = getPageId(findTabElement(rightClickEvent.target));
                var url = getTabUrlById(pageId);
                window.open(url);
            }
        }
    ]);
});