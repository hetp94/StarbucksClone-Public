//Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
//for details on configuring this project to bundle and minify static web assets.

//Write your JavaScript code.

var objArray = new Array();

if (localStorage.getItem("objKey") === null) {
    //...
}
else {
    objArray = JSON.parse(localStorage.getItem("objKey"));
    if (document.getElementById("cartCount") != null) {
        document.getElementById("cartCount").innerHTML = objArray.length;
    }

}
//Initialize and add the map
let map;
let service;
let infowindow;

async function initMap() {
    // The location of Uluru
    const position = { lat: 39.80048089441903, lng: -89.64614392160371 };
    console.log("Place Position");

    // Request needed libraries.


    //@ts-ignore
    const { Map } = await google.maps.importLibrary("maps");
    const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");

    console.log("Map and AdvancedMarkerElement");

    // The map, centered at Uluru
   
    map = new Map(document.getElementById("map"), {
        zoom: 13,
        center: position,
        mapId: "DEMO_MAP_ID",
    });

    console.log("Place Zoom Level setting");

    // The marker, positioned at Uluru
    const marker = new AdvancedMarkerElement({
        map: map,
        position: position,
        title: "Uluru",
    });
   

    var request = {
        query: 'starbucks',
        fields: ['name', 'geometry'],
    };

    service = new google.maps.places.PlacesService(map);
    console.log(1);
    service.findPlaceFromQuery(request, function (results, status) {
        if (status === google.maps.places.PlacesServiceStatus.OK) {
            for (var i = 0; i < results.length; i++) {
                createMarker(results[i]);
            }
            map.setCenter(results[0].geometry.location);
        }
    });

    google.maps.event.addListener(map, 'bounds_changed', function () {
        var lat0 = map.getBounds().getNorthEast().lat();
        var lng0 = map.getBounds().getNorthEast().lng();
        var lat1 = map.getBounds().getSouthWest().lat();
        var lng1 = map.getBounds().getSouthWest().lng();

        console.log(lat0);
        console.log(lng0);
        console.log(lat1);
        console.log(lng1);
    });

    console.log("Event listner for bound change");
}

initMap();



