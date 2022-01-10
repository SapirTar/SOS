

 // Initialize and add the map
      function initMap() {
        // The location of stock markets
          const telAviv = { lat: 32.064, lng: 34.77 };
          const newYork = { lat: 40.706, lng: -74.011 };
          const tokyo = { lat: 35.682, lng: 139.778 };

        //create map
        const map = new google.maps.Map(document.getElementById("map"), {
        zoom: 2,
            center: telAviv, newYork, tokyo
        });

        //locate the pins
        const markerTelAviv = new google.maps.Marker({
            position: telAviv,
          map: map,
        });
        const markerNewYork = new google.maps.Marker({
              position: newYork,
              map: map,
        });
          const markerTokyo = new google.maps.Marker({
              position: tokyo,
              map: map,
          });
          
}


// Set up our drawing context
const canvas = document.getElementById("canvas");
const context = canvas.getContext("2d");

const colour = "#000";
const strokeWidth = 2;
context.strokeStyle = colour;
context.lineWidth = strokeWidth;

// Drawing state
let latestPoint;
let drawing = false;



// Drawing functions

const continueStroke = newPoint => {
    context.beginPath();
    context.moveTo(latestPoint[0], latestPoint[1]);
    context.lineCap = "round";
    context.lineJoin = "round";
    context.lineTo(newPoint[0], newPoint[1]);
    context.stroke();

    latestPoint = newPoint;
};

// Event helpers

const startStroke = point => {
    drawing = true;
    latestPoint = point;
};

const BUTTON = 0b01;
const mouseButtonIsDown = buttons => (BUTTON & buttons) === BUTTON;

// Event handlers

const mouseMove = evt => {
    if (!drawing) {
        return;
    }
    continueStroke([evt.offsetX, evt.offsetY]);
};

const mouseDown = evt => {
    if (drawing) {
        return;
    }
    evt.preventDefault();
    canvas.addEventListener("mousemove", mouseMove, false);
    startStroke([evt.offsetX, evt.offsetY]);
};

const mouseEnter = evt => {
    if (!mouseButtonIsDown(evt.buttons) || drawing) {
        return;
    }
    mouseDown(evt);
};

const endStroke = evt => {
    if (!drawing) {
        return;
    }
    drawing = false;
    evt.currentTarget.removeEventListener("mousemove", mouseMove, false);
};

// Register event handlers

canvas.addEventListener("mousedown", mouseDown, false);
canvas.addEventListener("mouseup", endStroke, false);
canvas.addEventListener("mouseout", endStroke, false);
canvas.addEventListener("mouseenter", mouseEnter, false);


const getTouchPoint = evt => {
    if (!evt.currentTarget) {
        return [0, 0];
    }
    const rect = evt.currentTarget.getBoundingClientRect();
    const touch = evt.targetTouches[0];
    return [touch.clientX - rect.left, touch.clientY - rect.top];
};

const touchStart = evt => {
    if (drawing) {
        return;
    }
    evt.preventDefault();
    startStroke(getTouchPoint(evt));
};

const touchMove = evt => {
    if (!drawing) {
        return;
    }
    continueStroke(getTouchPoint(evt));
};

const touchEnd = evt => {
    drawing = false;
};

canvas.addEventListener("touchstart", touchStart, false);
canvas.addEventListener("touchend", touchEnd, false);
canvas.addEventListener("touchcancel", touchEnd, false);
canvas.addEventListener("touchmove", touchMove, false);