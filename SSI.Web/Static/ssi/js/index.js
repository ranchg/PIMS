var clientData = {};
$(function () {
    $.getJson({
        url: "/Index/GetClientData",
        async: false,
        type: "post",
        success: function (result) {
            top.clientData = result;
        }
    });
    addTabs({
        id: '0',
        title: '首页',
        close: false,
        url: '/Index/Default'
    });
    $('#clientUserMenu').sidebarMenu({ menu: top.clientData.menu });
    $('#clientUserName').text(top.clientData.user.User.F_Account);
    $('#clientUserHeadIconMin').attr("src", top.clientData.user.User.F_Head_Icon_Path);
    $('#clientUserHeadIconMax').attr("src", top.clientData.user.User.F_Head_Icon_Path);
    $('#clientUserRoleName').text(top.clientData.user.User.F_System_Mark == 1 ? "系统管理员" : $.getParamValues(top.clientData.user.Roles, "F_Name").join());
    $('#clientUserOrgName').text(top.clientData.user.User.F_System_Mark == 1 ? "系统管理员" :$.getParamValues(top.clientData.user.Orgs, "F_Name").join());
    $("#btnUserInfo").on("click", function (e) {
        $.modalOpen({
            title: "修改资料",
            content: "/Index/FormUserInfo"
        });
    });
    $("#btnUserPassword").on("click", function (e) {
        $.modalOpen({
            title: "修改密码",
            area: ['60%', '60%'],
            content: "/Index/FormUserPassword"
        });
    });
    $("#btnUserLogout").on("click", function (e) {
        $.confirmAjax({
            url: "/Index/Logout",
            success: function (result) {
                location.href = result.data.url;
            }
        });
    });
});