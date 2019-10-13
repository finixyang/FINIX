var clients = [];
$(function () {
    clients = $.clientsInit();
})
$.clientsInit = function () {
    var dataJson = {
        dataItems: [],
        organize: [],
        role: [],
        duty: [],
        authorizeMenu: [],
        authorizeButton: []
    };
    var initClients = function () {
        $.ajax({
            url: "/ClientsData/GetClientsDataJson",
            type: "get",
            dataType: "json",
            async: false,
            success: function (data) {
                dataJson.dataItems = data.dataItems;
                dataJson.organize = data.organize;
                dataJson.role = data.role;
                dataJson.duty = data.duty;
                dataJson.authorizeMenu = eval(data.authorizeMenu);
                dataJson.authorizeButton = data.authorizeButton;
            }
        });
    }

    var initOrderData = function () {
        $.ajax({
            url: "/ClientsData/GetOrderRelationDataJson",
            type: "get",
            dataType: "json",
            async: false,
            success: function (resData) {
                dataJson.user = resData.user;
                dataJson.orderFlows = resData.orderFlows;
                dataJson.customerItems = resData.customerItems;
                dataJson.productItems = resData.productItems;
                dataJson.financialFlows = resData.financialFlows;
            }
        });
    }

    var initAssetsData = function () {
        $.ajax({
            url: "/ClientsData/GetAssetsDataJson",
            type: "get",
            dataType: "json",
            async: false,
            success: function (resData) {
                dataJson.asset = resData.asset;
                dataJson.consumable = resData.consumable;
                dataJson.benefits = resData.benefits;
            }
        });
    }
    initClients();
    initOrderData();
    initAssetsData();
    return dataJson;
}
