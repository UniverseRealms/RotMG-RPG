package kabam.rotmg.game.view.components {
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.ui.tooltip.TextToolTip;
import com.company.util.AssetLibrary;

import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.Shape;

import flash.display.Sprite;
import flash.events.MouseEvent;
import flash.filters.GlowFilter;

import kabam.rotmg.game.model.StatModel;
import kabam.rotmg.messaging.impl.incoming.Text;
import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.text.view.TextFieldDisplay;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;

import org.osflash.signals.Signal;

import org.osflash.signals.natives.NativeSignal;

public class StatsView extends Sprite {

    private static const statsModel:Array = [new StatModel(TextKey.STAT_MODEL_ATTACK_SHORT, TextKey.STAT_MODEL_ATTACK_LONG, TextKey.STAT_MODEL_ATTACK_DESCRIPTION, true),
        new StatModel(TextKey.STAT_MODEL_DEFENSE_SHORT, TextKey.STAT_MODEL_DEFENSE_LONG, TextKey.STAT_MODEL_DEFENSE_DESCRIPTION, false),
        new StatModel(TextKey.STAT_MODEL_SPEED_SHORT, TextKey.STAT_MODEL_SPEED_LONG, TextKey.STAT_MODEL_SPEED_DESCRIPTION, true),
        new StatModel(TextKey.STAT_MODEL_DEXTERITY_SHORT, TextKey.STAT_MODEL_DEXTERITY_LONG, TextKey.STAT_MODEL_DEXTERITY_DESCRIPTION, true),
        new StatModel(TextKey.STAT_MODEL_VITALITY_SHORT, TextKey.STAT_MODEL_VITALITY_LONG, TextKey.STAT_MODEL_VITALITY_DESCRIPTION, true),
        new StatModel(TextKey.STAT_MODEL_WISDOM_SHORT, TextKey.STAT_MODEL_WISDOM_LONG, TextKey.STAT_MODEL_WISDOM_DESCRIPTION, true)];

    private var atkStat:StatView;
    private var defStat:StatView;
    private var spdStat:StatView;
    private var dexStat:StatView;
    private var vitStat:StatView;
    private var wisStat:StatView;

    private var contianerSprite:Sprite;
    private var coinBitMap:Bitmap; // add it the gold and cost icon
    private var costText:TextFieldDisplayConcrete;
    private var resetText:TextFieldDisplayConcrete;
    private var pointsText:TextFieldDisplayConcrete;

    public var resetSignal:Signal = new Signal();

    public function StatsView() {
        this.contianerSprite = new Sprite();
        addChild(this.contianerSprite);
        addStatView();
        positionAssets();
        addAssets();
        drawPoints();
        createReset();
    }

    private function addAssets():void {
        this.contianerSprite.addChild(atkStat);
        this.contianerSprite.addChild(defStat);
        this.contianerSprite.addChild(spdStat);
        this.contianerSprite.addChild(dexStat);
        this.contianerSprite.addChild(vitStat);
        this.contianerSprite.addChild(wisStat);
    }

    private function positionAssets():void {
        atkStat.x = 5; atkStat.y = -47;
        defStat.x = 5; defStat.y = -30;
        spdStat.x = 5; spdStat.y = -12;
        dexStat.x = 5; dexStat.y = 5;
        vitStat.x = 5; vitStat.y = 22;
        wisStat.x = 5; wisStat.y = 40;
    }

    private function addStatView():void {
        atkStat = new StatView("Att", setModel(0).name,
                setModel(0).description);
        defStat = new StatView("Def", setModel(1).name,
                setModel(1).description);
        spdStat = new StatView("Spd", setModel(2).name,
                setModel(2).description);
        dexStat = new StatView("Dex", setModel(3).name,
                setModel(3).description);
        vitStat = new StatView("Vit", setModel(4).name,
                setModel(4).description);
        wisStat = new StatView("Wis", setModel(5).name,
                setModel(5).description);
    }

    private function setModel(_arg1:Number):StatModel {
        return statsModel[_arg1];
    }

    private function drawPoints():void {
        pointsText = new TextFieldDisplayConcrete().setColor(0xFFFFFF).setSize(12).setBold(true);
        pointsText.setStringBuilder(new StaticStringBuilder("StatPoints:"));

        pointsText.x = 95;
        pointsText.y = -40;

        addChild(this.pointsText);
    }

    private function createReset():void {
        makeResetText(95, 36);
    }

    private function makeResetText(_arg1:Number, _arg2:Number):void {
        resetText = new TextFieldDisplayConcrete().setColor(0xFFFFFF).setSize(14).setBold(true);
        resetText.setStringBuilder(new StaticStringBuilder("Reset"));
        resetText.x = _arg1;
        resetText.y = _arg2;
        resetText.setTextWidth(30);
        makeCostText(_arg1 + 42, _arg2);

        resetText.addEventListener(MouseEvent.MOUSE_OVER, function(_arg1:MouseEvent):void {
            this.resetText.setColor(0xFCDF00);
        });
        resetText.addEventListener(MouseEvent.MOUSE_OUT, function (_arg1:MouseEvent):void {
            this.resetText.setColor(0xFFFFFF);
        });
        resetText.addEventListener(MouseEvent.CLICK, function (_arg1:MouseEvent):void {
            resetSignal.dispatch();
        });
        addChild(this.resetText);
    }

    private function makeCostText(_arg1:Number, _arg2:Number):void {
        var _local1:BitmapData = AssetLibrary.getImageFromSet("lofiObj3", 225);
        coinBitMap = new Bitmap(_local1);
        coinBitMap.scaleX = 2;
        coinBitMap.scaleY = 2;

        costText = new TextFieldDisplayConcrete().setColor(0xFFFFFF).setSize(13).setBold(false);
        costText.setStringBuilder(new StaticStringBuilder("500"));

        this.coinBitMap.x = _arg1;
        this.coinBitMap.y = _arg2;
        this.costText.x = _arg1 + coinBitMap.width + 7;
        this.costText.y = _arg2;

        addChild(this.coinBitMap);
        addChild(costText);
    }

    public function initPoints(_arg1:Player):void {
        this.pointsText.setStringBuilder(new StaticStringBuilder("StatPoints:" + _arg1.statpoint_));
    }

    public function drawValues(_arg1:Player):void {
        this.atkStat.drawVal(_arg1.attack_, _arg1.attackBoost_, _arg1.attackMax_);
        this.defStat.drawVal(_arg1.defense_, _arg1.defenseBoost_, _arg1.defenseMax_);
        this.spdStat.drawVal(_arg1.speed_, _arg1.speedBoost_, _arg1.speedMax_);
        this.dexStat.drawVal(_arg1.dexterity_, _arg1.dexterityBoost_, _arg1.dexterityMax_);
        this.vitStat.drawVal(_arg1.vitality_, _arg1.vitalityBoost_, _arg1.vitalityMax_);
        this.wisStat.drawVal(_arg1.wisdom_, _arg1.wisdomBoost_, _arg1.wisdomMax_)
    }
}
}