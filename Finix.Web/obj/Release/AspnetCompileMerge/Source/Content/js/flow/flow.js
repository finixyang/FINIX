var canvas = document.getElementById("myCanvas");
var cxt = canvas.getContext('2d');

var start = new Start(600, 40, "red");//新建开始对象
var step1 = new Step(600, 120, "green");//新建第一个步骤
start.drawBottomToTop(step1); //画箭头(从开始对象指向第一个步骤)

var step2 = new Step(600, 200, "#CCC");
step1.drawBottomToTop(step2);

var step3 = new Step(600, 280, "green");
step2.drawBottomToTop(step3);

var step4 = new Step(600, 360, "green");
step3.drawBottomToTop(step4);

var step5 = new Step(600, 440, "green");
step4.drawBottomToTop(step5);

var step6 = new Step(600, 520, "green");
step5.drawBottomToTop(step6);

var step7 = new Step(600, 600, "green");
step6.drawBottomToTop(step7);

var step8 = new Step(600, 680, "#CCC");
step7.drawBottomToTop(step8);

//var condition1 = new Condition(300, 200);//新建第一个条件对象
//var condition2 = new Condition(450, 200);
//var condition3 = new Condition(600, 200);
//var condition4 = new Condition(750, 200);
//var condition5 = new Condition(900, 200);

//step1.drawBottomToTop(condition1);//画箭头(从第一个步骤指向第一个条件)
//step1.drawBottomToTop(condition2);
//step1.drawBottomToTop(condition3);
//step1.drawBottomToTop(condition4);
//step1.drawBottomToTop(condition5);

//var step2 = new Step(450,295);
//var step3 = new Step(750,295);

//condition1.drawBottomToTop(step2);
//condition2.drawBottomToTop(step2);
//condition3.drawBottomToTop(step2);
//condition4.drawBottomToTop(step3);
//condition5.drawBottomToTop(step3);

//var condition6 = new Condition(300, 400);
//var condition7 = new Condition(450, 400);
//var condition8 = new Condition(750, 400);
//var condition9 = new Condition(900, 400);
//var condition10 = new Condition(450, 500);
//var condition11 = new Condition(900, 500);
//var condition12 = new Condition(450, 600);
//var condition13 = new Condition(900, 600);

//step2.drawBottomToTop(condition6);
//step3.drawBottomToTop(condition9);

//var step4 = new Step(300,725);
//var step5 = new Step(450,725);
//var step6 = new Step(600,725);
//var step7 = new Step(750,725);
//var step8 = new Step(900,725);

//condition6.drawBottomToTop(step4);
//condition6.drawRightToLeft(condition7);
//condition7.drawRightToTop(step6);
//condition7.drawBottomToTop(condition10);
//condition8.drawBottomToTop(step7);
//condition9.drawLeftToRight(condition8);
//condition9.drawBottomToTop(condition11);
//condition10.drawBottomToTop(condition12);
//condition11.drawBottomToTop(condition13);
//condition12.drawBottomToTop(step5);
//condition13.drawBottomToTop(step8);

/*canvas.onclick = function(e) { //给canvas添加点击事件
    e = e || event; //获取事件对象
    //获取事件在canvas中发生的位置
    var x = e.clientX - canvas.offsetLeft;
    var y = e.clientY - canvas.offsetTop;
    //如果事件位置在矩形区域中
    alert('x:' + x + "y:" + y);
     if(x>=rect.x&&x<=rect.x+rect.w&&y>=rect.y&&y<=rect.y+rect.h){
         alert('clicked')
     }
}*/
