//Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
//for details on configuring this project to bundle and minify static web assets.
//Write your JavaScript code.
//var objArray = new Array();
//if (localStorage.getItem("objKey") === null) {
//    //...
//}
//else {
//    objArray = JSON.parse(localStorage.getItem("objKey"));
//    if (document.getElementById("cartCount") != null) {
//        document.getElementById("cartCount").innerHTML = objArray.length;
//    }
//}
////Initialize and add the map
//let map;
//let service;
//let infowindow;
//async function initMap() {
//    // The location of Uluru
//    const position = { lat: 39.80048089441903, lng: -89.64614392160371 };
//    // Request needed libraries.
//    //@ts-ignore
//    const { Map } = await google.maps.importLibrary("maps");
//    const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");
//    // The map, centered at Uluru
//    map = new Map(document.getElementById("map"), {
//        zoom: 4,
//        center: position,
//        mapId: "DEMO_MAP_ID",
//    });
//    // The marker, positioned at Uluru
//    const marker = new AdvancedMarkerElement({
//        map: map,
//        position: position,
//        title: "Uluru",
//    });
//    google.maps.event.addListener(map, 'bounds_changed', function () {
//        var lat0 = map.getBounds().getNorthEast().lat();
//        var lng0 = map.getBounds().getNorthEast().lng();
//        var lat1 = map.getBounds().getSouthWest().lat();
//        var lng1 = map.getBounds().getSouthWest().lng();
//        console.log(lat0);
//        console.log(lng0);
//        console.log(lat1);
//        console.log(lng1);
//    });
//    var request = {
//        query: 'starbucks',
//        fields: ['name', 'geometry'],
//    };
//    service = new google.maps.places.PlacesService(map);
//    service.findPlaceFromQuery(request, function (results, status) {
//        if (status === google.maps.places.PlacesServiceStatus.OK) {
//            for (var i = 0; i < results.length; i++) {
//                createMarker(results[i]);
//            }
//            map.setCenter(results[0].geometry.location);
//        }
//    });
//}
//initMap();
//getLocation();
//function getLocation() {
//}
//var lat0 = map.getBounds().getNorthEast().lat();
//var lng0 = map.getBounds().getNorthEast().lng();
//var lat1 = map.getBounds().getSouthWest().lat();
//var lng1 = map.getBounds().getSouthWest().lng();
//console.log(lat0);
//    console.log(lng0);
//    console.log(lat1);
//    console.log(lng1);
//var map1 = new google.maps.Map(document.getElementById("map"), {
//    zoom: 10,
//    center: new google.maps.LatLng(lat, lng),
//    mapTypeId: google.maps.MapTypeId.ROADMAP
//});
//google.maps.event.addListener(map1, 'bounds_changed', function () {
//    var bounds = map.getBounds();
//    var ne = bounds.getNorthEast();
//    var sw = bounds.getSouthWest();
//    console.log(ne.lat());
//    console.log(ne.lng());
//    console.log(sw.lat());
//    console.log(sw.lng());
//});

var objArray = new Array();
$(document).ready(function () {

    if (localStorage.getItem("objKey") != null && localStorage.getItem("objKey") != "" && localStorage.getItem("objKey") != "null") {

        objArray = JSON.parse(localStorage.getItem("objKey"));

        var cartCount = document.getElementById("cartCount");
        if (cartCount != null) {
            cartCount.innerHTML = objArray.length;
        }

    }

    activeMenu();
    $('#toggleAboutUs').click(function () {
        alert(1);
        var collapseIcon = $('#collapseIcon');
        if (collapseIcon.hasClass('fa-chevron-down')) {
            collapseIcon.removeClass('fa-chevron-down').addClass('fa-chevron-up');
        } else {
            collapseIcon.removeClass('fa-chevron-up').addClass('fa-chevron-down');
        }
    });
});



function activeMenu() {
    var currentMenu = window.location.pathname;
   // console.log('currentMenu ' + currentMenu);

    if (currentMenu.includes("MenuType") || currentMenu.includes("Category") || currentMenu.includes("SubCategory")) {
        $("#MenuDropDownId").addClass("activeMenu");
    }
    else if (currentMenu.includes("CustomizationVisibilityCodes") || currentMenu.includes("CustomizationVisibilityOptions")
        || currentMenu.includes("CustomizationNews") || currentMenu.includes("CustomizationOptions")) {
        $("#CustomizationDropDownId").addClass("activeMenu");
    }
    else if (currentMenu.includes("Product")) {
        $("#ProductMenuId").addClass("activeMenu");
    }
    else if (currentMenu.includes("Layout")) {
        $("#LayoutMenuId").addClass("activeMenu");
    }

}