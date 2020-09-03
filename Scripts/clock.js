var canvas = document.getElementById("canvas");
var c = canvas.getContext("2d");

function drawClock() {
    c.clearRect(0, 0, canvas.width, canvas.height);
    c.fillStyle = 'transparent';
    c.fillRect(0, 0, canvas.width, canvas.height);

    var now = new Date(),
        h = now.getHours(),
        m = now.getMinutes(),
        s = now.getSeconds();

    h = addLeadingZeroWhenNecessary(h);
    m = addLeadingZeroWhenNecessary(m);
    s = addLeadingZeroWhenNecessary(s);

    var clockText = h + ':' + m + ':' + s,
        x = 10,
        y = 60;

    c.fillStyle = '#fa26a0';

    c.font = '50pt Arial';
    c.fillText(clockText, x, y);
    c.strokeText(clockText, x, y);
}

function addLeadingZeroWhenNecessary(s) {
    return (s < 10 ? '0' : '') + s;
}

drawClock();
setInterval(drawClock, 1000);