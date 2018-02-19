package com.company.assembleegameclient.ui {
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.ui.bitmapimages.EmptyBitmap;
import com.company.assembleegameclient.ui.tooltip.DummyToolTip;
import com.company.assembleegameclient.ui.tooltip.TextToolTip;

import flash.display.Bitmap;
import flash.display.BitmapData;

import flash.display.Sprite;
import flash.events.MouseEvent;

public class RuneSlot extends Sprite{

    private var ItemBitmap:Bitmap;

    private var objType:uint;
    private var toolTip:DummyToolTip;

    public function RuneSlot() {
        Init();
        this.addEventListener(MouseEvent.MOUSE_OVER, onHover);
        this.addEventListener(MouseEvent.MOUSE_OUT, onOut);
    }

    private function Init():void {
        DrawBackGround();
        InitBitmap();
        CreateToolTip();
    }

    private function DrawBackGround():void {
        graphics.clear();
        graphics.beginFill(0x454545);
        graphics.lineStyle(3, 0x666666);
        graphics.drawRect(0, 0, 42, 42);
        graphics.endFill();
    }

    private function InitBitmap():void {
        ItemBitmap = new Bitmap();
        ItemBitmap.x = 0;
        ItemBitmap.y = 0;

        addChild(ItemBitmap);
    }

    private function onHover(_arg1:MouseEvent):void {
        this.toolTip.visible = true;

    }

    private function onOut(_arg1:MouseEvent):void {
        this.toolTip.visible = false;
    }

    private function CreateToolTip():void {
        this.toolTip = new DummyToolTip(0x363636, 150, 100, "Empty", "Runes give you extra buffs and abilities.");
        this.toolTip.x = 10;
        this.toolTip.y = 5;
        this.toolTip.visible = false;
        addChild(this.toolTip);
    }

    public function Draw(_arg1:Player):void {
        if (_arg1.runeSlot_ != 0){
            objType = _arg1.runeSlot_;
            var  _local1:BitmapData = ObjectLibrary.getRedrawnTextureFromType(_arg1.runeSlot_, 80, true);
            ItemBitmap.bitmapData = _local1;
            ItemBitmap.x = (-7);
            ItemBitmap.y = (-7);

            var _local2:XML = ObjectLibrary.xmlLibrary_[_arg1.runeSlot_];
            var _local4:String = ObjectLibrary.typeToDisplayId_[objType];

            this.toolTip.setTitleText(_local4);
            this.toolTip.setDescription(_local2.Description);
        } else {
            ItemBitmap = new EmptyBitmap();
            objType = 0x00;
        }
        this.toolTip.setStagePos(this.mouseX, this.mouseY);
    }
}
}
