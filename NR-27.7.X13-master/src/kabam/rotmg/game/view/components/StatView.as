package kabam.rotmg.game.view.components {
import com.company.assembleegameclient.map.Square;
import com.company.assembleegameclient.ui.tooltip.TextToolTip;
import com.company.assembleegameclient.ui.tooltip.ToolTip;

import flash.display.Bitmap;
import flash.display.BitmapData;

import flash.display.Sprite;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;
import flash.geom.Rectangle;
import flash.text.TextField;
import flash.text.TextFieldAutoSize;

import kabam.rotmg.game.model.StatModel;

import kabam.rotmg.text.model.TextKey;

import kabam.rotmg.text.view.TextFieldDisplay;

import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;

import org.osflash.signals.Signal;

import org.osflash.signals.natives.NativeSignal;

public class StatView extends Sprite {

    private var nameText:TextFieldDisplayConcrete;
    private var valText:TextFieldDisplayConcrete;
    public var addText:TextFieldDisplayConcrete;

    private var toolTip:TextToolTip;
    public var incSignal:Signal = new Signal(int);

    private var fullName:String;
    private var description:String;
    private var valColor_:uint = 0xB3B3B3;
    private var val_:int = -1;
    private var boost_:int = -1;
    private var statType:int;

    public function StatView(_arg1:String, _arg2:String, _arg3:String) {
        this.toolTip = new TextToolTip(0x363636, 0x9B9B9B, "", "", 200);
        super();
        fullName = _arg2;
        description = _arg3;
        drawTexts(_arg1);
    }

    private function drawTexts(_arg1:String):void {
        nameText = new TextFieldDisplayConcrete().setColor(0xFFFFFF).setSize(13).setBold(false);
        valText = new TextFieldDisplayConcrete().setColor(valColor_).setSize(13).setBold(false);
        addText = new TextFieldDisplayConcrete().setColor(0xFFFFFF).setSize(16).setBold(true);
        nameText.setStringBuilder(new LineBuilder().setParams(_arg1 + ":"));
        valText.setStringBuilder(new StaticStringBuilder("null"));
        addText.setStringBuilder(new StaticStringBuilder("+"));

        initStatType(_arg1);

        addText.addEventListener(MouseEvent.MOUSE_OVER, onHover);
        addText.addEventListener(MouseEvent.MOUSE_OUT, onOut);
        addText.addEventListener(MouseEvent.CLICK, function (_arg1:MouseEvent):void {
            sendIncrement();
        });

        nameText.x = 2;
        valText.x = 40;
        addText.x = 70;
        addText.y = -2;

        addChild(nameText);
        addChild(valText);
        addChild(addText);
    }

    private function initStatType(_arg1:String):void {
        switch(_arg1.toLowerCase()){
            case "atk": statType = 2; break;
            case "def": statType = 3; break;
            case "spd": statType = 4; break;
            case "dex": statType = 5; break;
            case "vit": statType = 6; break;
            case "wis": statType = 7; break;
            default: statType = -1; break;
        }
    }

    private function sendIncrement():void {
        incSignal.dispatch(statType);
    }

    private function onHover(_arg1:MouseEvent):void {
        addText.setColor(0xFCDF00);
    }

    private function onOut(_arg1:MouseEvent):void {
        addText.setColor(0xFFFFFF);
    }

    public function addToolTip():void {
        toolTip.setTitle(new LineBuilder().setParams(this.fullName));
        toolTip.setText(new LineBuilder().setParams(this.description));

        if (!stage.contains(this.toolTip)) {
            stage.addChild(this.toolTip);
        }
    }

    public function removeToolTip():void {
        if (this.toolTip.parent != null) {
            this.toolTip.parent.removeChild(this.toolTip);
        }
    }

    public function drawVal(_arg1:int, _arg2:int, _arg3:int):void {
        if (_arg1 == this.val_ && _arg2 == this.boost_) {
            return;
        }
        var _local1:uint;
        this.val_ = _arg1;
        this.boost_ = _arg2;

        if ((_arg1 - _arg2) >= _arg3) {
            _local1 = 0xFCDF00;
        } else {
            if (boost_ > 0) {
                _local1 = 6206769;
            } else {
                _local1 = 0xB3B3B3;
            }
        }

        if (valColor_ != _local1) {
            valColor_ = _local1;
            this.valText.setColor(valColor_);
        }
        var _local2:String = this.val_.toString();

        if (this.boost_ > 0) {
            _local2 = (_local2 + "[" + boost_ + "]");
        }
        this.valText.setStringBuilder(new StaticStringBuilder(_local2));
    }
}
}
