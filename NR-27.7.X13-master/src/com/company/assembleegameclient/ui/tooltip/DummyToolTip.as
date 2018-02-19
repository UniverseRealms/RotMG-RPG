package com.company.assembleegameclient.ui.tooltip {
import com.company.assembleegameclient.map.Square;

import flash.display.Shape;

import flash.display.Sprite;
import flash.text.TextField;
import flash.text.TextFieldAutoSize;
import flash.text.TextFormatAlign;


import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
import kabam.rotmg.ui.view.SignalWaiter;

public class DummyToolTip extends Sprite{

    private var waiter:SignalWaiter = new SignalWaiter();

    private var titleTextField:TextFieldDisplayConcrete;
    private var desText:TextFieldDisplayConcrete;

    private var line:Shape;

    private var _width:int;
    private var _height:int;

    public function DummyToolTip(color:uint, width:int, height:int, ttext:String, dtext:String) {
        drawBackGround(color, width, height);
        drawTitleText(ttext);
        drawDescription(dtext);
        _width = width;
        _height = height;
        setLine();
    }

    private function drawBackGround(_arg1:uint, _arg2:int, _arg3:int):void {
        graphics.clear();
        graphics.beginFill(_arg1);
        graphics.lineStyle(2, 0x969696);
        graphics.drawRect(0,0, _arg2, _arg3);
        graphics.endFill();
    }

    private function drawTitleText(_arg1:String):void {
        this.titleTextField = new TextFieldDisplayConcrete().setSize(12).setColor(0xFFFFFF).setBold(true);
        this.titleTextField.setStringBuilder(new StaticStringBuilder(_arg1));

        this.titleTextField.x = 5;
        this.titleTextField.y = 5;

        addChild(this.titleTextField);
    }

    private function drawDescription(_arg1:String):void {
        this.desText = new TextFieldDisplayConcrete().setSize(9).setColor(0xFFFFFF).setBold(false);
        this.desText.setStringBuilder(new LineBuilder().setParams(String(_arg1)));
        this.desText.x = 5;
        this.desText.y = 30;

        addChild(this.desText);
    }

    private function setLine():void {
        this.line = new Shape();

        this.line.graphics.clear();
        this.line.graphics.lineStyle(2, 0xBFBFBF);
        this.line.graphics.moveTo(0, 23);
        this.line.graphics.lineTo(_width, 23);
        this.line.graphics.lineStyle();

        addChild(this.line);
    }

    public function setTitleText(_arg1:String):void {
        this.titleTextField.setStringBuilder(new StaticStringBuilder(_arg1));
    }

    public function setDescription(_arg1:String):void {
        this.desText.setStringBuilder(new LineBuilder().setParams(String(_arg1)));
        this.desText.setMultiLine(true);
        this.desText.setWordWrap(true);
        this.desText.setTextWidth(_width);
        this.desText.setTextHeight(_height - line.x);
    }

    public function setStagePos(_x:int, _y:int):void {
        this.x = _x;
        this.y = _y;
    }
}
}
