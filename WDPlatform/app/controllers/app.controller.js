(function (angular) {
	'use strict';

	/**
	 * AppController
	 * The App Controller is responsible for exposing the number of online users to the view.
	 * 
	 * The controller will invoke the 'getUserCount' on initialization.
	 * Finally, the controller will listen for the 'sendUserCount' 
	 *    event from the hub and update the user count sent by the hub.
	 * 
	 * @param {AppHub}
	 */
	AppController.$inject = ['AppHub', '$http'];
	function AppController(AppHub, $scope, $location) {

		// Internal reference
		var self = this;

		// user count property we'll expose to the view
		this.usersOnline = 0;

		this.roomNumber;
		this.userName;
		this.openGame = false;
		this.game = {};
		this.players = [];

		// Connection status property we'll expose to the view
		this.connectionStatus = AppHub.connectionStatus;

		// Internal hub event handler, simply gets the count from the hub 
		///    and updates the usersOnline property of the controller
		//
		function updateCount(count) {
			self.usersOnline = count;
		}

		this.create = function () {
		    AppHub.invoke('createRoom', this.userName).then(function (roomNumber) {
                console.log('Room Create successfully');
                self.roomNumber = roomNumber;
                self.openGame = true;
		    })
		};

		this.join = function () {
		    AppHub.invoke('joinRoom', this.roomNumber, this.userName).then(function (roomNumber) {
		        console.log('Room Create successfully');
		        self.openGame = true;
		    })
		};

		AppHub.on('refreshGame', getNewUser)

		function getNewUser(game) {
		    self.game = game;
		    console.log(self.game);
		    self.players = Object.keys(game.players);
		}

		// Receive the sendUserCount event from the AppHub
		AppHub.on('sendUserCount', updateCount);

		// Here we'll invoke the 'getUserCount' event and hook into the promise.
		// When the promise resolves (thanks to then), we call the updateCount method.
		//
		AppHub.invoke('getUserCount').then(updateCount);


	}

	// Register the 'AppController' to the 'app' module.
	angular.module('app').controller('AppController', AppController);

})(window.angular);