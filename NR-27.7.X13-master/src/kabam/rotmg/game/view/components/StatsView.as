package kabam.rotmg.game.view.components {
import com.company.assembleegameclient.objects.Player;

import flash.display.Sprite;
import flash.events.MouseEvent;
import flash.filters.GlowFilter;

import kabam.rotmg.game.model.StatModel;
import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.text.view.TextFieldDisplay;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
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

    public var contianerSprite:Sprite;
    private var resetText:TextFieldDisplayConcrete;//do l8r
    private var pointsText:TextFieldDisplayConcrete;

    public function StatsView() {
        this.contianerSprite = new Sprite();
        addChild(this.contianerSprite);
        addStatView();
        positionAssets();
        addAssets();
        drawPoints();
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
        atkStat.x = 5; atkStat.y = -50;
        defStat.x = 5; defStat.y = -33;
        spdStat.x = 5; spdStat.y = -15;
        dexStat.x = 5; dexStat.y = 2;
        vitStat.x = 5; vitStat.y = 19;
        wisStat.x = 5; wisStat.y = 37;
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
        pointsText = new TextFieldDisplayConcrete().setColor(0xFFFFFF).setSize(18).setBold(true);
        pointsText.setStringBuilder(new StaticStringBuilder("StatPoint(s):"));

        pointsText.x = 90;
        pointsText.y = -35;

        addChild(this.pointsText);
    }

    public function initPoints(_arg1:Player):void {
        this.pointsText.setStringBuilder(new StaticStringBuilder("StatPoint(s):" + _arg1.statpoint_));
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