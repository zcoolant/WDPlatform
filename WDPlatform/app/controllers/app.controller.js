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
		this.isCreater = false;
		this.isPlayer = false;
		this.isStart = false;
		this.players = [];
		this.cards = [];
		this.question = "";
        this.status = "WD Platform Beta"

		// Connection status property we'll expose to the view
		this.connectionStatus = AppHub.connectionStatus;

		// Internal hub event handler, simply gets the count from the hub 
		///    and updates the usersOnline property of the controller
		//
		function updateCount(count) {
			self.usersOnline = count;
		}

        //create new room
		this.create = function () {
		    AppHub.invoke('createRoom', this.userName).then(function (roomNumber) {
                console.log('Room Create successfully');
                self.roomNumber = roomNumber;
                self.isCreater = true;
                self.isStart = false;
                self.status = "Room-" + self.roomNumber + ": Waiting for players"
		    })
		};

        //join a rrom
		this.join = function () {
		    AppHub.invoke('joinRoom', this.roomNumber, this.userName).then(function (roomNumber) {
		        console.log('Room Create successfully');
		        self.isCreater = false;
		        self.isPlayer = true;
		        self.isStart = false;
		        self.status = "Room-"+self.roomNumber+": Waiting for players"
		    })
		};

		this.start = function () {
		    AppHub.invoke('startGame', this.roomNumber).then(function () {
		        console.log('Game Start successfully');
		        self.isStart = true;
		    })
		}

		AppHub.on('refreshPlayers', function (players) {
		    console.log(players);
		    self.players = players;
		});

		AppHub.on('reloadCards', function (cards) {
		    console.log(cards);
		    self.isStart = true;
		    self.cards = cards;
		});

		AppHub.on('reloadQuestion', function (question) {
		    console.log(question);
		    self.status = "Q: " + question;
		    self.question = question;
		});

		// Receive the sendUserCount event from the AppHub
		AppHub.on('sendUserCount', updateCount);

		// Here we'll invoke the 'getUserCount' event and hook into the promise.
		// When the promise resolves (thanks to then), we call the updateCount method.
		//
		AppHub.invoke('getUserCount').then(updateCount);

		//this.cards = [ 'The invisible hand' , 'waiting until marriage' ];

		//this.players = { 'Tong': 0, 'Erin': 0 };

        //this.question = "Instead of coal, Santa now gives the bad children ?"

	}

	// Register the 'AppController' to the 'app' module.
	angular.module('app').controller('AppController', AppController);

})(window.angular);