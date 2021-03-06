﻿package kabam.rotmg.ui.view {
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.ui.RewardNotification;
import com.company.assembleegameclient.ui.RuneSlot;
import com.company.assembleegameclient.ui.TradePanel;
import com.company.assembleegameclient.ui.panels.InteractPanel;
import com.company.assembleegameclient.ui.panels.itemgrids.EquippedGrid;
import com.company.util.GraphicsUtil;
import com.company.util.SpriteUtil;
import com.gskinner.motion.GTween;

import flash.display.GraphicsPath;
import flash.display.GraphicsSolidFill;
import flash.display.IGraphicsData;
import flash.display.Sprite;
import flash.events.Event;
import flash.events.TimerEvent;
import flash.geom.Point;
import flash.utils.Timer;

import kabam.rotmg.game.view.components.TabStripView;
import kabam.rotmg.messaging.impl.incoming.TradeAccepted;
import kabam.rotmg.messaging.impl.incoming.TradeChanged;
import kabam.rotmg.messaging.impl.incoming.TradeStart;
import kabam.rotmg.minimap.view.MiniMapImp;

public class HUDView extends Sprite implements UnFocusAble {

    private const BG_POSITION:Point = new Point(0, 0);
    private const MAP_POSITION:Point = new Point(4, 4);
    private const CHARACTER_DETAIL_PANEL_POSITION:Point = new Point(0, 154);
    private const STAT_METERS_POSITION:Point = new Point(12, 192);
    private const EQUIPMENT_INVENTORY_POSITION:Point = new Point(14, 304);
    private const TAB_STRIP_POSITION:Point = new Point(7, 346);
    private const INTERACT_PANEL_POSITION:Point = new Point(0, 500);
    private const RUNE_SLOT_POSITION:Point = new Point(13, 260);

    private var background:CharacterWindowBackground;
    private var miniMap:MiniMapImp;
    private var equippedGrid:EquippedGrid;
    private var statMeters:StatMetersView;
    private var characterDetails:CharacterDetailsView;
    private var equippedGridBG:Sprite;
    private var player:Player;
    public var tabStrip:TabStripView;
    public var interactPanel:InteractPanel;
    public var tradePanel:TradePanel;
    private var rewardNotification:RewardNotification;
    private var runeSlot:RuneSlot;

    public function HUDView() {
        this.createAssets();
        this.addAssets();
        this.positionAssets();
    }

    private function createAssets():void {
        this.background = new CharacterWindowBackground();
        this.miniMap = new MiniMapImp(192, 150);
        this.tabStrip = new TabStripView();
        this.characterDetails = new CharacterDetailsView();
        this.statMeters = new StatMetersView();
        this.runeSlot = new RuneSlot();
    }

    private function addAssets():void {
        addChild(this.background);
        addChild(this.miniMap);
        addChild(this.tabStrip);
        addChild(this.characterDetails);
        addChild(this.statMeters);
    }

    private function positionAssets():void {
        this.background.x = this.BG_POSITION.x;
        this.background.y = this.BG_POSITION.y;
        this.miniMap.x = this.MAP_POSITION.x;
        this.miniMap.y = this.MAP_POSITION.y;
        this.tabStrip.x = this.TAB_STRIP_POSITION.x;
        this.tabStrip.y = this.TAB_STRIP_POSITION.y;
        this.characterDetails.x = this.CHARACTER_DETAIL_PANEL_POSITION.x;
        this.characterDetails.y = this.CHARACTER_DETAIL_PANEL_POSITION.y;
        this.statMeters.x = this.STAT_METERS_POSITION.x;
        this.statMeters.y = this.STAT_METERS_POSITION.y;
        this.runeSlot.x = this.RUNE_SLOT_POSITION.x;
        this.runeSlot.y = this.RUNE_SLOT_POSITION.y;
    }

    public function setPlayerDependentAssets(_arg1:GameSprite):void {
        this.player = _arg1.map.player_;
        this.createEquippedGridBackground();
        this.createEquippedGrid();
        this.createInteractPanel(_arg1);
        addRewardNotifivation();
        addRuneSlot();
    }

    private function addRuneSlot():void {
        addChild(this.runeSlot);
    }

    private function addRewardNotifivation():void {
        this.rewardNotification = new RewardNotification();
        this.rewardNotification.x -= this.rewardNotification.width;
        this.rewardNotification.y = this.characterDetails.y;
        this.rewardNotification.visible = false;
        this.rewardNotification.alpha = 0;

        addChild(this.rewardNotification);
    }

    public function SetReward(_arg1:uint):void {
        this.rewardNotification.setItemBitmap(_arg1);
        this.rewardNotification.visible = true;
        if (!this.stage.contains(this.rewardNotification)){
            addChild(this.rewardNotification);
            this.rewardNotification.visible = true;
            this.rewardNotification.alpha = 0;
        }
        new GTween(this.rewardNotification, 1, {"alpha": 1});
        var _local1:Timer = new Timer(500, 0);
        _local1.addEventListener(TimerEvent.TIMER_COMPLETE, onTimer);
    }

    private function onTimer(_arg1:TimerEvent):void {
        this.rewardNotification.visible = false;
        this.rewardNotification.alpha = 0;
    }

    private function createInteractPanel(_arg1:GameSprite):void {
        this.interactPanel = new InteractPanel(_arg1, this.player, 200, 100);
        this.interactPanel.x = this.INTERACT_PANEL_POSITION.x;
        this.interactPanel.y = this.INTERACT_PANEL_POSITION.y;
        addChild(this.interactPanel);
    }

    private function createEquippedGrid():void {
        this.equippedGrid = new EquippedGrid(this.player, this.player.slotTypes_, this.player);
        this.equippedGrid.x = this.EQUIPMENT_INVENTORY_POSITION.x;
        this.equippedGrid.y = this.EQUIPMENT_INVENTORY_POSITION.y;
        addChild(this.equippedGrid);
    }

    private function createEquippedGridBackground():void {
        var _local3:Vector.<IGraphicsData>;
        var _local1:GraphicsSolidFill = new GraphicsSolidFill(0x676767, 1);
        var _local2:GraphicsPath = new GraphicsPath(new Vector.<int>(), new Vector.<Number>());
        _local3 = new <IGraphicsData>[_local1, _local2, GraphicsUtil.END_FILL];
        GraphicsUtil.drawCutEdgeRect(0, 0, 178, 46, 6, [0, 0, 0, 0], _local2);
        this.equippedGridBG = new Sprite();
        this.equippedGridBG.x = (this.EQUIPMENT_INVENTORY_POSITION.x - 3);
        this.equippedGridBG.y = (this.EQUIPMENT_INVENTORY_POSITION.y - 3);
        this.equippedGridBG.graphics.drawGraphicsData(_local3);
        addChild(this.equippedGridBG);
    }

    public function draw(_arg1:Player):void {
        if (this.equippedGrid) {
            this.equippedGrid.draw();
        }
        if (this.interactPanel) {
            this.interactPanel.draw();
        }
        this.runeSlot.Draw(_arg1);
    }

    public function startTrade(_arg1:AGameSprite, _arg2:TradeStart):void {
        if (!this.tradePanel) {
            this.tradePanel = new TradePanel(_arg1, _arg2);
            this.tradePanel.y = 200;
            this.tradePanel.addEventListener(Event.CANCEL, this.onTradeCancel);
            addChild(this.tradePanel);
            this.setNonTradePanelAssetsVisible(false);
        }
    }

    private function setNonTradePanelAssetsVisible(_arg1:Boolean):void {
        this.characterDetails.visible = _arg1;
        this.statMeters.visible = _arg1;
        this.tabStrip.visible = _arg1;
        this.equippedGrid.visible = _arg1;
        this.equippedGridBG.visible = _arg1;
        this.interactPanel.visible = _arg1;
    }

    public function tradeDone():void {
        this.removeTradePanel();
    }

    public function tradeChanged(_arg1:TradeChanged):void {
        if (this.tradePanel) {
            this.tradePanel.setYourOffer(_arg1.offer_);
        }
    }

    public function tradeAccepted(_arg1:TradeAccepted):void {
        if (this.tradePanel) {
            this.tradePanel.youAccepted(_arg1.myOffer_, _arg1.yourOffer_);
        }
    }

    private function onTradeCancel(_arg1:Event):void {
        this.removeTradePanel();
    }

    private function removeTradePanel():void {
        if (this.tradePanel) {
            SpriteUtil.safeRemoveChild(this, this.tradePanel);
            this.tradePanel.removeEventListener(Event.CANCEL, this.onTradeCancel);
            this.tradePanel = null;
            this.setNonTradePanelAssetsVisible(true);
        }
    }

    public function setMiniMapFocus(object:GameObject) : void {
        this.miniMap.setFocus(object);
    }

}
}
