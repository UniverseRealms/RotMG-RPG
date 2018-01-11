package kabam.rotmg.game.view.components {
import com.company.assembleegameclient.objects.Player;

import flash.events.MouseEvent;

import kabam.lib.net.api.MessageProvider;

import kabam.lib.net.impl.SocketServer;
import kabam.rotmg.messaging.impl.GameServerConnection;
import kabam.rotmg.messaging.impl.outgoing.IncrementStat;

import kabam.rotmg.ui.signals.UpdateHUDSignal;
import kabam.rotmg.ui.view.StatsDockedSignal;

import robotlegs.bender.bundles.mvcs.Mediator;

public class StatsMediator extends Mediator {

    [Inject]
    public var view:StatsView;
    [Inject]
    public var updateHUD:UpdateHUDSignal;
    [Inject]
    public var statsUndocked:StatsUndockedSignal;
    [Inject]
    public var statsDocked:StatsDockedSignal;
    [Inject]
    public var socketServer:SocketServer;
    [Inject]
    public var messages:MessageProvider;


    override public function initialize():void {
        view.resetSignal.add(onReset);
        updateHUD.add(onUpdate);
    }

    override public function destroy():void {
    }

    private function onReset():void {
        var _local1:IncrementStat = (this.messages.require(GameServerConnection.STATINCREMENT) as IncrementStat);
        _local1.statType  = -1;
        _local1.reset = true;
        this.socketServer.queueMessage(_local1);
    }

    private function onUpdate(_arg1:Player):void {
        this.view.drawValues(_arg1);
        this.view.initPoints(_arg1);
    }
}
}