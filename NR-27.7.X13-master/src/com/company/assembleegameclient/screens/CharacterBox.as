package com.company.assembleegameclient.screens {
import com.company.assembleegameclient.appengine.CharacterStats;
import com.company.assembleegameclient.appengine.SavedCharacter;
import com.company.assembleegameclient.ui.tooltip.ClassToolTip;
import com.company.assembleegameclient.ui.tooltip.ToolTip;
import com.company.assembleegameclient.util.AnimatedChar;
import com.company.assembleegameclient.util.Currency;
import com.company.assembleegameclient.util.FameUtil;
import com.company.rotmg.graphics.FullCharBoxGraphic;
import com.company.rotmg.graphics.LockedCharBoxGraphic;
import com.company.rotmg.graphics.StarGraphic;
import com.company.util.AssetLibrary;
import com.gskinner.motion.GTween;

import flash.display.Bitmap;
import flash.display.Sprite;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;
import flash.geom.ColorTransform;
import flash.text.TextFieldAutoSize;

import kabam.rotmg.core.model.PlayerModel;
import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;
import kabam.rotmg.util.components.LegacyBuyButton;

import org.osflash.signals.natives.NativeSignal;

public class CharacterBox extends Sprite {

    private static const fullCT:ColorTransform = new ColorTransform(0.8, 0.8, 0.8);
    private static const emptyCT:ColorTransform = new ColorTransform(0.2, 0.2, 0.2);

    private var SaleTag:Class;
    public var playerXML_:XML = null;
    public var charStats_:CharacterStats;
    public var model:PlayerModel;
    public var available_:Boolean;
    private var graphicContainer_:Sprite;
    private var graphic_:Sprite;
    private var bitmap_:Bitmap;
    private var statusText_:TextFieldDisplayConcrete;
    private var classNameText_:TextFieldDisplayConcrete;
    private var cost:int = 0;
    public var characterSelectClicked_:NativeSignal;
    private var charBackGround:Sprite;

    public function CharacterBox(_arg1:XML, _arg2:CharacterStats, _arg3:PlayerModel, _arg4:Boolean = false) {
        var _local5:Sprite;
        this.SaleTag = CharacterBox_SaleTag;
        super();
        this.model = _arg3;
        this.playerXML_ = _arg1;
        this.charStats_ = _arg2;
        this.available_ = ((_arg4) || (_arg3.isLevelRequirementsMet(this.objectType())));
        if (!this.available_) {
            this.graphic_ = new LockedCharBoxGraphic();
            this.graphic_.scaleX = 2;
            this.graphic_.scaleY = 2;
            this.cost = this.playerXML_.UnlockCost;
        }
        else {
            this.graphic_ = new FullCharBoxGraphic();
            this.graphic_.scaleX = 2;
            this.graphic_.scaleY = 2;
        }
        this.graphicContainer_ = new Sprite();
        addChild(this.graphicContainer_);
        makeCharBackGround();
        this.graphicContainer_.addChild(this.graphic_);
        this.characterSelectClicked_ = new NativeSignal(this.graphicContainer_, MouseEvent.CLICK, MouseEvent);
        this.bitmap_ = new Bitmap(null);
        this.setImage(AnimatedChar.DOWN, AnimatedChar.STAND, 0);
        this.graphic_.addChild(this.bitmap_);
        this.classNameText_ = new TextFieldDisplayConcrete().setSize(14).setColor(0xFFFFFF).setAutoSize(TextFieldAutoSize.CENTER).setTextWidth(this.graphic_.width).setBold(false);
        this.classNameText_.setStringBuilder(new LineBuilder().setParams(ClassToolTip.getDisplayId(this.playerXML_)));
        this.classNameText_.filters = [new DropShadowFilter(0, 0, 0, 1, 4, 4)];
        this.graphic_.addChild(this.classNameText_);
        this.setStatusButton();
        if (this.available_) {
            _local5 = this.getStars(FameUtil.numStars(_arg3.getBestFame(this.objectType())), FameUtil.STARS.length);
            _local5.y = 170;
            _local5.x = 70;
            _local5.filters = [new DropShadowFilter(0, 0, 0, 1, 4, 4)];
            this.graphicContainer_.addChild(_local5);
            this.classNameText_.x = -54;
            this.classNameText_.y = 68;
        }
    }

    private function makeCharBackGround():void {
        this.charBackGround = new Sprite();
        this.charBackGround.graphics.beginFill(0x818181);
        this.charBackGround.graphics.drawCircle(0, 0, 33);
        this.charBackGround.graphics.endFill();
        this.charBackGround.x = 53;
        this.charBackGround.y = 35;

        this.graphic_.addChild(this.charBackGround);
    }

    public function objectType():int {
        return (int(this.playerXML_.@type));
    }

    public function getTooltip():ToolTip {
        return (new ClassToolTip(this.playerXML_, this.model, this.charStats_));
    }

    public function setOver(_arg1:Boolean):void {
        if (!this.available_) {
            return;
        }
        if (_arg1) {
            transform.colorTransform = new ColorTransform(1.2, 1.2, 1.2);
        }
        else {
            transform.colorTransform = new ColorTransform(1, 1, 1);
        }
    }

    private function setImage(_arg1:int, _arg2:int, _arg3:Number):void {
        this.bitmap_.bitmapData = SavedCharacter.getImage(null, this.playerXML_, _arg1, _arg2, _arg3, this.available_, false);
        this.bitmap_.x = 22;
    }

    private function getStars(_arg1:int, _arg2:int):Sprite {
        var _local5:Sprite;
        var _local3:Sprite = new Sprite();
        var _local4:int;
        var _local6:int;
        while (_local4 < _arg1) {
            _local5 = new StarGraphic();
            _local5.x = _local6;
            _local5.transform.colorTransform = fullCT;
            _local3.addChild(_local5);
            _local6 = (_local6 + _local5.width);
            _local4++;
        }
        while (_local4 < _arg2) {
            _local5 = new StarGraphic();
            _local5.x = _local6;
            _local5.transform.colorTransform = emptyCT;
            _local3.addChild(_local5);
            _local6 = (_local6 + _local5.width);
            _local4++;
        }
        return (_local3);
    }

    private function setStatusButton():void {
        this.statusText_ = new TextFieldDisplayConcrete().setSize(14).setColor(0xFF0000).setAutoSize(TextFieldAutoSize.CENTER).setBold(true).setTextWidth(this.graphic_.width);
        this.statusText_.setStringBuilder(new LineBuilder().setParams(TextKey.LOCKED));
        this.statusText_.filters = [new DropShadowFilter(0, 0, 0, 1, 4, 4)];
        this.statusText_.y = 58;
    }
}
}
