package kabam.rotmg.ui.view {
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.objects.Player;

import kabam.lib.net.api.MessageProvider;
import kabam.lib.net.impl.SocketServer;
import kabam.rotmg.messaging.impl.GameServerConnection;

import kabam.rotmg.messaging.impl.outgoing.IncrementStat;

import kabam.rotmg.ui.model.HUDModel;
import kabam.rotmg.ui.signals.UpdateHUDSignal;

import robotlegs.bender.bundles.mvcs.Mediator;

public class StatMetersMediator extends Mediator {

    [Inject]
    public var view:StatMetersView;
    [Inject]
    public var hudModel:HUDModel;
    [Inject]
    public var updateHUD:UpdateHUDSignal;
    [Inject]
    public var socketServer:SocketServer;
    [Inject]
    public var messages:MessageProvider;


    override public function initialize():void {
        this.view.incrementSignal.add(this.onIncrement);
        this.updateHUD.add(this.onUpdateHUD);
    }

    override public function destroy():void {
        this.updateHUD.add(this.onUpdateHUD);
    }

    private function onUpdateHUD(_arg1:Player):void {
        this.view.update(_arg1);
    }

    private function onIncrement(_arg1:int):void {
        IncStat(_arg1);
    }

    private function IncStat(_arg1:int):void {
        var _local1:IncrementStat = (this.messages.require(GameServerConnection.STATINCREMENT) as IncrementStat);
        _local1.statType  = _arg1;
        _local1.reset = false;
        this.socketServer.queueMessage(_local1);
    }
}
}
