function drawRoundRect(x, y, w, h, color, text) {
    var r = h / 2;
    cxt.beginPath();
    cxt.moveTo(x + r, y);
    cxt.arcTo(x + w, y, x + w, y + h, r);
    cxt.arcTo(x + w, y + h, x, y + h, r);
    cxt.arcTo(x, y + h, x, y, r);
    cxt.arcTo(x, y, x + w, y, r);
	cxt.fillStyle = color;	
	cxt.fill(); 
    cxt.closePath();
    cxt.stroke();
	cxt.strokeStyle = "black";
	cxt.strokeText(text, x + 40, y + 28);
	cxt.textAlign = "center";
	cxt.textBaseline = "middle";
}


/**
 * 圆角矩形开始对象
 * @param {Object} x
 * @param {Object} y
 */
function Start(x, y, color, text) {
    this.h = 50;
    this.w = 2 * this.h;
    this.x = x;
    this.y = y;
	this.color = color;
	drawRoundRect(x - this.w / 2, y - this.h / 2, this.w, this.h, color, text);
}

/**
 * 矩形步骤对象
 * @param {Object} x
 * @param {Object} y
 */
function Step(x, y, color, text) {
    this.flag = "step";
    this.h = 50;
    this.w = 2 * this.h;
    this.x = x;
    this.y = y;
	this.color = color;
	cxt.fillStyle = color;
    cxt.fillRect(x - this.w / 2, y - this.h / 2, this.w, this.h);	
	cxt.strokeStyle = "black";
	cxt.strokeText(text, x, y);
	cxt.textAlign = "center";
	cxt.textBaseline = "middle";
}

Start.prototype.drawBottomToTop = function(obj) {
	var arrow,x1, y1, x2, y2;
	x1 = this.x;
	y1 = this.y + this.h / 2;
	x2 = obj.x;
    if(obj.flag == "step") {
		y2 = obj.y - obj.h / 2;
    } else if(obj.flag == "condition") {
		y2 = obj.y - obj.l;
    }
    arrow = new Arrow(x1, y1, x2, y2);
	arrow.setColor(this.color);
    arrow.drawBottomToTop(cxt);
}

Step.prototype.drawBottomToTop = function(obj) {
	var arrow,x1, y1, x2, y2;
	x1 = this.x;
	y1 = this.y + this.h / 2;
	x2 = obj.x;
    if(obj.flag == "step") {
		y2 = obj.y - obj.h / 2;
    } else if(obj.flag == "condition") {
		y2 = obj.y - obj.l;
    }
    arrow = new Arrow(x1, y1, x2, y2);
	arrow.setColor(this.color);
    arrow.drawBottomToTop(cxt);
}


/**
 * 菱形条件对象
 * @param {Object} x
 * @param {Object} y
 */

/*function drawRhombus(x, y, l, color) {
    cxt.beginPath();
	cxt.fillStyle = color;	
    cxt.moveTo(x, y + l);
    cxt.lineTo(x - l * 2, y);
    cxt.lineTo(x, y - l);
    cxt.lineTo(x + l * 2, y);
	cxt.fill(); 
    cxt.closePath();
    cxt.stroke();
}
function Condition(x, y) {
    this.flag = "condition";
    this.l = 30;
    this.x = x;
    this.y = y;
    drawRhombus(x, y, this.l);
}

Condition.prototype.drawBottomToTop = function(obj) {
    if(obj.flag == "step") {
        var arrow = new Arrow(this.x, this.y + this.l, obj.x, obj.y - obj.h / 2);
        arrow.drawBottomToTop(cxt);
    } else if(obj.flag == "condition") {
        var arrow = new Arrow(this.x, this.y + this.l, obj.x, obj.y - obj.l);
        arrow.drawBottomToTop(cxt);
    }
}

Condition.prototype.drawRightToTop = function(obj) {
    if(obj.flag == "step") {
        var arrow = new Arrow(this.x + this.l * 2, this.y, obj.x, obj.y - obj.h / 2);
        arrow.drawLeftOrRightToTop(cxt);
    } else if(obj.flag == "condition") {
        var arrow = new Arrow(this.x + this.l * 2, this.y, obj.x, obj.y - obj.l);
        arrow.drawLeftOrRightToTop(cxt);
    }
}

Condition.prototype.drawLeftToTop = function(obj) {
    if(obj.flag == "step") {
        var arrow = new Arrow(this.x - this.l * 2, this.y, obj.x, obj.y - obj.h / 2);
        arrow.drawLeftOrRightToTop(cxt);
    } else if(obj.flag == "condition") {
        var arrow = new Arrow(this.x - this.l * 2, this.y, obj.x, obj.y - obj.l);
        arrow.drawLeftOrRightToTop(cxt);
    }
}

Condition.prototype.drawRightToLeft = function(obj) {
    if(obj.flag == "step") {
        var arrow = new Arrow(this.x + this.l * 2, this.y, obj.x - this.w / 2, obj.y);
        arrow.drawLeftToRightOrRightToLeft(cxt);
    } else if(obj.flag == "condition") {
        var arrow = new Arrow(this.x + this.l * 2, this.y, obj.x - this.l * 2, obj.y);
        arrow.drawLeftToRightOrRightToLeft(cxt);
    }
}

Condition.prototype.drawLeftToRight = function(obj) {
    if(obj.flag == "step") {
        var arrow = new Arrow(this.x - this.l * 2, this.y, obj.x + this.w / 2, obj.y);
        arrow.drawLeftToRightOrRightToLeft(cxt);
    } else if(obj.flag == "condition") {
        var arrow = new Arrow(this.x - this.l * 2, this.y, obj.x + this.l * 2, obj.y);
        arrow.drawLeftToRightOrRightToLeft(cxt);
    }
}*/