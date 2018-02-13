'use strict';

document.addEventListener('DOMContentLoaded', function(){
    alert('hello world');
});

document.getElementById('Generate').addEventListener('click', function () {
    var thisElement = document.getElementById('Track');
    var newTrack = thisElement.cloneNode(true);
    newTrack.getElementById('Duration');

    newTrack.childNodes();

    
    

    thisElement.parentNode.appendChild(newTrack);
    //thisElement.parentNode.insertBefore(newTrack, thisElement);
}, false);


document.getElementById('Delete').addEventListener('click', function () {
    var thisElement = document.getElementById('Track');
    var newTrack = thisElement.cloneNode(true);

    thisElement.parentNode.appendChild(newTrack);
}, false);