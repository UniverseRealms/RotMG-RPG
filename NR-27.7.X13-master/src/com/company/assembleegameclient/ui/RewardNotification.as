package com.company.assembleegameclient.ui {
import com.company.assembleegameclient.objects.ObjectLibrary;

import flash.display.Bitmap;
import flash.display.Sprite;
import flash.events.MouseEvent;

import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;


public class RewardNotification extends Sprite {

    private var itemBitmap:Bitmap;
    private var closeText:TextFieldDisplayConcrete;
    private var congratsText:TextFieldDisplayConcrete;

    public function RewardNotification() {
        DrawBackGround();
        createClose();
        makeRewardText();
        createItemBitmap();
    }

    private function DrawBackGround():void {
        this.graphics.beginFill(0x363636);
        this.graphics.lineStyle(1, 0xFFFFFF);
        this.graphics.drawRect(0, 0, 140, 100);
        this.graphics.endFill();
    }

    private function createClose():void {
        this.closeText = new TextFieldDisplayConcrete().setSize(16).setBold(true).setColor(0xFFFFFF);
        this.closeText.setStringBuilder(new StaticStringBuilder("X"));

        this.closeText.x = 126;
        this.closeText.y = 5;
        this.closeText.addEventListener(MouseEvent.CLICK, onClose);

        addChild(this.closeText);
    }

    private function makeRewardText():void {
        this.congratsText = new TextFieldDisplayConcrete().setSize(12).setBold(true).setColor(0xFFFFFF);
        congratsText.setStringBuilder(new StaticStringBuilder("Congratulations!"));

        this.congratsText.x = 20;
        this.congratsText.y = 83;

        addChild(this.congratsText);
    }

    private function onClose(_arg1:MouseEvent):void {
        this.parent.removeChild(this);
    }

    private function createItemBitmap():void {
        this.itemBitmap = new Bitmap();
        this.itemBitmap.x = 0;
        this.itemBitmap.y = 0;

        addChild(this.itemBitmap);
    }

    public function setItemBitmap(_arg1:uint):void {
        this.itemBitmap.bitmapData = ObjectLibrary.getRedrawnTextureFromType(_arg1, 60, true);
        this.itemBitmap.scaleX = 1.7;
        this.itemBitmap.scaleY = 1.7;
        this.itemBitmap.x = this.width/2 - this.itemBitmap.width/2;
        this.itemBitmap.y = this.height/2 - this.itemBitmap.height/2 - 7;
    }
}
}
