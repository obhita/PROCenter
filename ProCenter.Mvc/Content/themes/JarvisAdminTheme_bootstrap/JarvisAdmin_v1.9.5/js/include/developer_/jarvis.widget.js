/*         
     __                   .__        
    |__|____ __________  _|__| ______
    |  \__  \\_  __ \  \/ /  |/  ___/
    |  |/ __ \|  | \/\   /|  |\___ \ 
/\__|  (____  /__|    \_/ |__/____  >
\______|    \/                    \/ 

   Copyright 2013 - Jarvis : Smart Admin Template

 * This script is part of an item on wrapbootstrap.com
 * https://wrapbootstrap.com/user/myorange
 * 
 * Date 	 : Jan 2013
 * Updated	 : 19/10/2012
 * Dependency: jQuery UI core, json2(ie7)  
 * 
 * ************************************************************* *
 * 
 * Jarvis Widgets (AKA Power Widgets), is orgiginally created by 
 * Mark (www.creativemilk.net). This script may NOT be RESOLD or 
 * REDISTRUBUTED under any circumstances, and is only to be used 
 * with this purchased copy of Jarvis Smart Admin Template. 
 * 
 * ************************************************************* */

;(function($, window, document, undefined){
	
	//"use strict"; // jshint ;_;
	
	var pluginName = 'jarvisWidgets';
	
	function Plugin(element, options){
		/**
		* Variables.
		**/			
		this.obj             = $(element);
		this.o               = $.extend({}, $.fn[pluginName].defaults, options);
		this.objId           = this.obj.attr('id');
		this.pwCtrls         = '.jarviswidget-ctrls'
		this.widget          = this.obj.find(this.o.widgets);
		this.toggleClass     = this.o.toggleClass.split('|');
		this.editClass       = this.o.editClass.split('|');
		this.fullscreenClass = this.o.fullscreenClass.split('|');
		this.customClass     = this.o.customClass.split('|');
		
		this.init();
	};

	Plugin.prototype = {
	
		/**	
		* Important settings like storage and touch support. 
		* 
		* @param:
		**/	
		_settings: function(){
			
            var self = this;
				
			//*****************************************************************//
			//////////////////////// LOCALSTORAGE CHECK /////////////////////////
			//*****************************************************************//
					
				storage = !!function() {
				  var result,
					  uid = +new Date;
				  try {
					localStorage.setItem(uid, uid);
					result = localStorage.getItem(uid) == uid;
					localStorage.removeItem(uid);
					return result;
				  } catch(e) {}
				}() && localStorage;
				
			//*****************************************************************//
			/////////////////////////// SET/GET KEYS ////////////////////////////
			//*****************************************************************//
			
				if(storage && self.o.localStorage){  
					keySettings    = 'Plugin_settings_'+location.pathname+'_'+self.objId;
					getKeySettings = localStorage.getItem(keySettings);
						
					keyPosition    = 'Plugin_position_'+location.pathname+'_'+self.objId;
					getKeyPosition = localStorage.getItem(keyPosition);
				}	
		
			//*****************************************************************//
			////////////////////////// TOUCH SUPPORT ////////////////////////////
			//*****************************************************************//
			
				/**
				* Check for touch support and set right click events.
				**/
				if(('ontouchstart' in window) || window.DocumentTouch && document instanceof DocumentTouch){
					clickEvent = 'click tap';	
				}else{
					clickEvent = 'click';	
				}
			
		},

		/**
		* Function for the indicator image.
		* 
		* @param:
		**/
		_runLoaderWidget: function(elm){
			var self = this;
			if(self.o.indicator === true){
			  elm
			  .parents(self.o.widgets)
			  .find('.jarviswidget-loader')
			  .stop(true, true)
			  .fadeIn(100)
			  .delay(self.o.indicatorTime)
			  .fadeOut(100);	
			}
		},	
			
		/**	
		* Create a fixed timestamp. 
		* 
		* @param: t | date | Current date.
		**/				
		_getPastTimestamp: function(t) {
			
			var self = this;
			
			var da   = new Date(t);
			
			/**
			* Get and set the date and time.
			**/
			tsMonth   = da.getMonth() + 1;// index based
			tsDay     = da.getDate();
			tsYear    = da.getFullYear();
			tsHours   = da.getHours();
			tsMinutes = da.getMinutes();
			tsSeconds = da.getUTCSeconds();
			
			/**
			* Checking for one digit values, if so add an zero.
			**/
			if(tsMonth   < 10) { var tsMonth   = '0'+tsMonth   }
			if(tsDay     < 10) { var tsDay     = '0'+tsDay     }
			if(tsHours   < 10) { var tsHours   = '0'+tsHours   }
			if(tsMinutes < 10) { var tsMinutes = '0'+tsMinutes }
			if(tsSeconds < 10) { var tsSeconds = '0'+tsSeconds }
			
			/**
			* The output, how you want it.
			**/
			var format = self.o.timestampFormat
						 .replace(/%d%/g, tsDay)
						 .replace(/%m%/g, tsMonth)
						 .replace(/%y%/g, tsYear) 
						 .replace(/%h%/g, tsHours)
						 .replace(/%i%/g, tsMinutes)
						 .replace(/%s%/g, tsSeconds); 
	
			return format;
		},
		
		/**	
		* AJAX load File, which get and shows the . 
		* 
		* @param: awidget | object  | The widget.
		* @param: file    | file    | The file thats beeing loaded.
		* @param: loader  | object  | The widget.
		**/	
		_loadAjaxFile: function(awidget, file, loader){
			
			var self = this	

			awidget.find('.jarviswidget-ajax-placeholder').load(file, function(response, status, xhr){
				
				/**
				* If action runs into an error display an error msg.
				**/
				if(status == "error"){
				  $(this).html('<div class="inner-spacer">'+self.o.labelError + '<b>'+xhr.status + " " + xhr.statusText+'</b></div>');
				}
				
				/**
				* Run if there are no errors.
				**/
				if(status == "success"){
					
					/**	
					* Show a timestamp.
					**/	
					var aPalceholder = awidget.find(self.o.timestampPlaceholder);
					
					if(aPalceholder.length){
											
						aPalceholder.html(self._getPastTimestamp(new Date()));
					}

					/**	
					* Run the callback function.
					**/	
					if(typeof self.o.afterLoad == 'function'){
						self.o.afterLoad.call(this, awidget);
					}
				}
			});

			/**
			* Run function for the indicator image.
			**/
			self._runLoaderWidget(loader);
							
		},
						
		/**
		* Save all settings to the localStorage. 
		* 
		* @param: 
		**/	
		_saveSettingsWidget: function(){
			
			var self = this;
			
			self._settings();
			
			if(storage && self.o.localStorage){ 
				var storeSettings = [];
				
				self.obj.find(self.o.widgets).each(function(){
					var storeSettingsStr          = {};
					storeSettingsStr['id']        = $(this).attr('id');
					storeSettingsStr['style']     = $(this).attr('data-widget-attstyle');
					storeSettingsStr['title']     = $(this).children('header').children('h2').text();
					storeSettingsStr['hidden']    = ($(this).is(':hidden') ? 1 : 0);
					storeSettingsStr['collapsed'] = ($(this).hasClass('jarviswidget-collapsed') ? 1 : 0);
					storeSettings.push(storeSettingsStr);
				});	
					
				var storeSettingsObj = JSON.stringify( {'widget':storeSettings} );

				/* Place it in the storage(only if needed) */
				if(getKeySettings != storeSettingsObj){
					localStorage.setItem(keySettings, storeSettingsObj); 
				}
			}
			
			/**	
			* Run the callback function.
			**/	
			if(typeof self.o.onSave == 'function'){
				self.o.onSave.call(this, null,storeSettingsObj);
			}	
		},
				
		/**	
		* Save positions to the localStorage. 
		* 
		* @param: 
		**/	
		_savePositionWidget: function(){
			
			var self = this;
			
			self._settings();
			
			if(storage && self.o.localStorage){ 
				var mainArr = [];
				
				self.obj.find(self.o.grid+'.sortable-grid').each(function(){
					var subArr = [];
					$(this).children(self.o.widgets).each(function(){
						var subObj   = {};
						subObj['id'] = $(this).attr('id');
						subArr.push(subObj);
					});
					var out = {'section':subArr}
					mainArr.push(out);
				});	
					
				var storePositionObj = JSON.stringify( {'grid':mainArr} );

				/* Place it in the storage(only if needed) */
				if(getKeyPosition != storePositionObj){
					localStorage.setItem(keyPosition, storePositionObj, null); 
				}
			}

			/**	
			* Run the callback function.
			**/	
			if(typeof self.o.onSave == 'function'){
				self.o.onSave.call(this, storePositionObj);
			}	
		},
			
		/**	
		* Code that we run at the start. 
		* 
		* @param:
		**/	
		init: function(){
			
			var self = this;
			
			self._settings();

			/**
			* Force users to use an id(it's needed for the local storage).
			**/
			if(!$('#'+self.objId).length){
				alert('It looks like your using a class instead of an ID, dont do that!')	
			}
			
			/**
			* Add RTL support.
			**/
			if(self.o.rtl === true){
				$('body').addClass('rtl');
			}
			
			/**
			* This will add an extra class that we use to store the
			* widgets in the right order.(savety)
			**/

			$(self.o.grid).each(function(){
				if($(this).find(self.o.widgets).length){
					$(this).addClass('sortable-grid');	
				}
			});
					
		//*****************************************************************//
		//////////////////////// SET POSITION WIDGET ////////////////////////
		//*****************************************************************//						
				   
			/**	
			* Run if data is present.
			**/
			if(storage && self.o.localStorage && getKeyPosition){
					
				var jsonPosition = JSON.parse(getKeyPosition);
				
				/**	
				* Loop the data, and put every widget on the right place.
				**/	
				for(var key in jsonPosition.grid){
					var changeOrder = self.obj.find(self.o.grid+'.sortable-grid').eq(key);
					 for(var key2 in jsonPosition.grid[key].section){								
						changeOrder.append($('#'+jsonPosition.grid[key].section[key2].id));
					}
				}
		
			}

		//*****************************************************************//
		/////////////////////// SET SETTINGS WIDGET /////////////////////////
		//*****************************************************************//						
  
			/**	
			* Run if data is present.
			**/
			if(storage && self.o.localStorage && getKeySettings){
					
				var jsonSettings = JSON.parse(getKeySettings);
				
				/**	
				* Loop the data and hide/show the widgets and set the inputs in
				* panel to checked(if hidden) and add an indicator class to the div.
				* Loop all labels and update the widget titles.
				**/			
				for(var key in jsonSettings.widget){
					var widgetId = $('#'+jsonSettings.widget[key].id);
					
					/**	
					* Set a style(if present).
					**/	
					if(jsonSettings.widget[key].style){		
						widgetId
						.addClass(jsonSettings.widget[key].style)
						.attr('data-widget-attstyle',''+jsonSettings.widget[key].style+'');
					}
					
					/**	
					* Hide/show widget.
					**/			
					if(jsonSettings.widget[key].hidden == 1){
						widgetId.hide(1);
					}else{
						widgetId
						.show(1)
						.removeAttr('data-widget-hidden');				
					}
					
					/**	
					* Toggle content widget.
					**/	
					if(jsonSettings.widget[key].collapsed == 1){
						widgetId
						.addClass('jarviswidget-collapsed')
						.children('div')
						.hide(1);
					}
					
					/**	
					* Update title widget (if needed).
					**/	
					if(widgetId.children('header').children('h2').text() != jsonSettings.widget[key].title){	
						widgetId
						.children('header')
						.children('h2')
						.text(jsonSettings.widget[key].title);
					}	
				}
			}
				
		//*****************************************************************//
		////////////////////////// LOOP AL WIDGETS //////////////////////////
		//*****************************************************************//
			
			/**
			* This will add/edit/remove the settings to all widgets
			**/
			self.widget.each(function(){
				
				var tWidget    = $(this);
				var thisHeader = $(this).children('header');
		
				/**
				* Dont double wrap(check).
				**/
				if(!thisHeader.parent().attr('role')){
				
					/**
					* Hide the widget if the dataset 'widget-hidden' is set to true.
					**/
					if(tWidget.data('widget-hidden') === true){
						
						tWidget.hide();
					}
										
					/**
					* Hide the content of the widget if the dataset 
					* 'widget-collapsed' is set to true.



					**/
					if(tWidget.data('widget-collapsed') === true){
						tWidget
						.addClass('jarviswidget-collapsed')
						.children('div')
						.hide();
					}
				
					/**
					* Check for the dataset 'widget-icon' if so get the icon 
					* and attach it to the widget header.
					**/
					if(tWidget.data('widget-icon')){						
						thisHeader.prepend('<i class="jarviswidget-icon '+tWidget.data('widget-icon')+'"></i>');
					}
					
					/**
					* Add a delete button to the widget header (if set to true).
					**/
					if(self.o.customButton === true && tWidget.data('widget-custombutton') === undefined && self.customClass[0].length != 0){	
						var customBtn = '<a href="#" class="button-icon jarviswidget-custom-btn"><i class="'+self.customClass[0]+'"></i></a>';
					}else{
							customBtn = '';
					}
					
					/**
					* Add a delete button to the widget header (if set to true).
					**/
					if(self.o.deleteButton === true && tWidget.data('widget-deletebutton') === undefined){	
						var deleteBtn = '<a href="#" class="button-icon jarviswidget-delete-btn"><i class="'+self.o.deleteClass+'"></i></a>';
					}else{
							deleteBtn = '';
					}
					
					/**
					* Add a delete button to the widget header (if set to true).
					**/
					if(self.o.editButton === true && tWidget.data('widget-editbutton') === undefined){	
						var editBtn = '<a href="#" class="button-icon jarviswidget-edit-btn"><i class="'+self.editClass[0]+'"></i></a>';
					}else{
							editBtn = '';
					}
					
					/**
					* Add a delete button to the widget header (if set to true).
					**/
					if(self.o.fullscreenButton === true && tWidget.data('widget-fullscreenbutton') === undefined){	
						var fullscreenBtn = '<a href="#" class="button-icon jarviswidget-fullscreen-btn"><i class="'+self.fullscreenClass[0]+'"></i></a>';
					}else{
							fullscreenBtn = ''; 
					}
															
					/**
					* Add a toggle button to the widget header (if set to true).
					**/
					if(self.o.toggleButton === true && tWidget.data('widget-togglebutton') === undefined){	
						if(tWidget.data('widget-collapsed') === true || tWidget.hasClass('jarviswidget-collapsed')){
							var toggleSettings = self.toggleClass[1];
						}else{
								toggleSettings = self.toggleClass[0];
						}
						var toggleBtn = '<a href="#" class="button-icon jarviswidget-toggle-btn"><i class="'+toggleSettings+'"></i></a>';
					}else{
							toggleBtn = '';							
					}
					
					/**
					* Add a refresh button to the widget header (if set to true).
					**/
					if(self.o.refreshButton === true && tWidget.data('widget-refreshbutton') != false && tWidget.data('widget-load')){
						var refreshBtn = '<a href="#" class="button-icon jarviswidget-refresh-btn"><i class="'+self.o.refreshButtonClass+'"></i></a>';	
					}else{
							refreshBtn = '';
					}
					
					/**
					* Set the buttons order.
					**/
					var formatButtons = self.o.buttonOrder
										.replace(/%refresh%/g, refreshBtn)
										.replace(/%delete%/g, deleteBtn)
										.replace(/%custom%/g, customBtn)
										.replace(/%fullscreen%/g, fullscreenBtn)
										.replace(/%edit%/g, editBtn)
										.replace(/%toggle%/g, toggleBtn); 
				
					/**
					* Add a button wrapper to the header.
					**/
					if(refreshBtn != '' || deleteBtn != '' || customBtn != '' || fullscreenBtn != '' || editBtn != '' || toggleBtn != ''){								
						thisHeader.append('<div class="jarviswidget-ctrls">'+formatButtons+'</div>');
					}
				
					/**
					* Adding a helper class to all sortable widgets, this will be 
					* used to find the widgets that are sortable, it will skip the widgets 
					* that have the dataset 'widget-sortable="false"' set to false.
					**/
					if(self.o.sortable === true && tWidget.data('widget-sortable') === undefined){
						tWidget.addClass('jarviswidget-sortable');
					}
									
					/**
					* If the edit box is present copy the title to the input.
					**/
					if(tWidget.find(self.o.editPlaceholder).length){
						tWidget
						.find(self.o.editPlaceholder)
						.find('input')
						.val(thisHeader.children('h2').text());	
					}
	
					/**
					* Prepend the image to the widget header.
					**/
					thisHeader.append('<span class="jarviswidget-loader"></span>');
			
					/**
					* Adding roles to some parts.
					**/
					tWidget
					.attr('role','widget')
					.children('div')
					.attr('role','content')
					.prev('header')
					.attr('role','heading')
					.children('div')
					.attr('role','menu');						
				}
			});
			
			/**
			* Hide all buttons if option is set to true.
			**/
			if(self.o.buttonsHidden === true){
				$(self.o.pwCtrls).hide();
			}

		//******************************************************************//
		//////////////////////////////// AJAX ////////////////////////////////
		//******************************************************************//
			
			/**
			* Loop all ajax widgets.
			**/
			self.obj.find('[data-widget-load]').each(function(){
				
				/**	
				* Variables.
				**/
				var thisItem       = $(this);
				var thisItemHeader = thisItem.children();
				var pathToFile     = thisItem.data('widget-load');
				var reloadTime     = thisItem.data('widget-refresh') * 1000;								
				var ajaxLoader     = thisItem.children();
				
				if(!thisItem.find('.jarviswidget-ajax-placeholder').length){
					
					/**	
					* Append a AJAX placeholder.
					**/
					thisItem.children('div:first').append('<div class="jarviswidget-ajax-placeholder"><span style="margin:10px">'+self.o.loadingLabel+'</span></div>');
									
					/**	
					* If widget has a reload time refresh the widget, if the value
					* has been set to 0 dont reload.
					**/
					if(thisItem.data('widget-refresh') > 0){
						
						/**	
						* Load file on start.
						**/
						self._loadAjaxFile(thisItem, pathToFile, thisItemHeader);
						
						/**	
						* Set an interval to reload the content every XXX seconds.
						**/
						setInterval(function(){	
						
							self._loadAjaxFile(thisItem, pathToFile, thisItemHeader);
						},reloadTime);
					}else{
						
						/**	
						* Load the content just once.
						**/
						self._loadAjaxFile(thisItem, pathToFile, thisItemHeader);
						
					}
				}
			});	

		//******************************************************************//
		////////////////////////////// SORTABLE //////////////////////////////
		//******************************************************************//

			/**
			* jQuery UI soratble, this allows users to sort the widgets. 
			* Notice that this part needs the jquery-ui core to work.
			**/
			if(self.o.sortable === true && jQuery.ui){
				var sortItem = self.obj.find('.sortable-grid').not('[data-widget-excludegrid]');
				sortItem.sortable({
					items:                sortItem.find('.jarviswidget-sortable'),
					connectWith:          sortItem,
					placeholder:          self.o.placeholderClass,
					cursor:               'move',
					revert:               true, 
					opacity:              self.o.opacity,
					delay:                200,
					cancel:               '.button-icon, #jarviswidget-fullscreen-mode > div',
					zIndex:               10000,
					handle:               self.o.dragHandle,
					forcePlaceholderSize: true,
					forceHelperSize:      true,
					update: function(event, ui){
						/* run pre-loader in the widget */
						self._runLoaderWidget(ui.item.children());	
						/* store the positions of the plugins */
						self._savePositionWidget();
						/**	
						* Run the callback function.
						**/	
						if(typeof self.o.onChange == 'function'){
							self.o.onChange.call(this, ui.item);
						}															
					}
				});	
			}

		//*****************************************************************//
		////////////////////////// BUTTONS VISIBLE //////////////////////////
		//*****************************************************************//
			
			/**
			* Show and hide the widget control buttons, the buttons will be 
			* visible if the users hover over the widgets header. At default the 
			* buttons are always visible.
			**/
			if(self.o.buttonsHidden === true){
										
				/**
				* Show and hide the buttons.
				**/
				self.widget.children('header').hover(function(){
					$(this)
					.children(self.o.pwCtrls)
					.stop(true, true)
					.fadeTo(100,1.0);
				},function(){
					$(this)
					.children(self.o.pwCtrls)
					.stop(true, true)
					.fadeTo(100,0.0);		
				});
			}	
	

		//*****************************************************************//
		///////////////////////// CLICKEVENTS //////////////////////////
		//*****************************************************************//	
		
			self._clickEvents();

		//*****************************************************************//
		///////////////////// DELETE LOCAL STORAGE KEYS /////////////////////
		//*****************************************************************//	

			/**	
			* Delete the settings key.
			**/
			$(self.o.deleteSettingsKey).on(clickEvent, this, function(e){
				if(storage && self.o.localStorage){
					var cleared = confirm(self.o.settingsKeyLabel);
					if(cleared){ 					
						localStorage.removeItem(keySettings);
					}
				}
				e.preventDefault();		
			});
			
			/**	
			* Delete the position key.
			**/
			$(self.o.deletePositionKey).on(clickEvent, this, function(e){
				if(storage && self.o.localStorage){
					var cleared = confirm(self.o.positionKeyLabel);
					if(cleared){ 					
						localStorage.removeItem(keyPosition);
					}
				}
				e.preventDefault();		
			});	

		//*****************************************************************//
		///////////////////////// CREATE NEW KEYS  //////////////////////////
		//*****************************************************************//	

			/**
			* Create new keys if non are present.
			**/
			if(storage && self.o.localStorage){ 
			
				/**
				* If the local storage key (keySettings) is empty or 
				* does not excite, create one and fill it.
				**/	
				if(getKeySettings === null || getKeySettings.length < 1){
					self._saveSettingsWidget();					
				}

				/**
				* If the local storage key (keyPosition) is empty or 
				* does not excite, create one and fill it.
				**/		
				if(getKeyPosition === null || getKeyPosition.length < 1){					
					self._savePositionWidget();					
				}
			}
						
		},

		/**
		* All of the click events.
		*
		* @param:
		**/
		_clickEvents: function(){
			
			var self = this;
			
			self._settings();

			//*****************************************************************//
			/////////////////////////// TOGGLE WIDGETS //////////////////////////
			//*****************************************************************//
				
				/**
				* Allow users to toggle the content of the widgets.
				**/
				self.widget.on(clickEvent, '.jarviswidget-toggle-btn', function(e){
					
					var tWidget = $(this);
					var pWidget = tWidget.parents(self.o.widgets);
	
					/**						
					* Run function for the indicator image.
					**/
					self._runLoaderWidget(tWidget);
			
					/**
					* Change the class and hide/show the widgets content.
					**/ 	
					if(pWidget.hasClass('jarviswidget-collapsed')){
						tWidget
						.children()
						.removeClass(self.toggleClass[1])
						.addClass(self.toggleClass[0])							
						.parents(self.o.widgets)
						.removeClass('jarviswidget-collapsed')
						.children('[role=content]')
						.slideDown(self.o.toggleSpeed, function(){
							self._saveSettingsWidget();
						});
					}else{
						tWidget
						.children()
						.removeClass(self.toggleClass[0])
						.addClass(self.toggleClass[1])
						.parents(self.o.widgets)
						.addClass('jarviswidget-collapsed')
						.children('[role=content]')
						.slideUp(self.o.toggleSpeed, function(){
							self._saveSettingsWidget();
						});
					}
					

					/**	
					* Run the callback function.
					**/	
					if(typeof self.o.onToggle == 'function'){
						self.o.onToggle.call(this, pWidget);
					}
				
					e.preventDefault();
				});

			//*****************************************************************//
			///////////////////////// FULLSCREEN WIDGETS ////////////////////////
			//*****************************************************************//
		 
				/**
				* Set fullscreen height function.
				**/	
				function heightFullscreen(){
					if($('#jarviswidget-fullscreen-mode').length){
					 
						/**						
						* Setting height variables.
						**/
						var heightWindow = $(window).height();
						var heightHeader = $('#jarviswidget-fullscreen-mode').find(self.o.widgets).children('header').height();
	
						/**						
						* Setting the height to the right widget.
						**/
						$('#jarviswidget-fullscreen-mode')
						.find(self.o.widgets)
						.children('div')
						.height( heightWindow - heightHeader - 3 );
					}
				}
					
				/**
				* On click go to fullscreen mode.
				**/
				self.widget.on(clickEvent,'.jarviswidget-fullscreen-btn', function(e){
	
					var thisWidget        = $(this).parents(self.o.widgets);
					var thisWidgetContent = thisWidget.children('div');
					
					/**						
					* Run function for the indicator image.
					**/
					self._runLoaderWidget($(this));
					
					/**						
					* Wrap the widget and go fullsize.
					**/
					if($('#jarviswidget-fullscreen-mode').length){
						
						/**						
						* Remove class from the body.
						**/
						$('.nooverflow').removeClass('nooverflow');	
						
						/**						
						* Unwrap the widget, remove the height, set the right 
						* fulscreen button back, and show all other buttons.
						**/
						thisWidget
						.unwrap('<div>')
						.children('div')
						.removeAttr('style')
						.end()
						.find('.jarviswidget-fullscreen-btn')
						.children()
						.removeClass(self.fullscreenClass[1])
						.addClass(self.fullscreenClass[0])
						.parents(self.pwCtrls)
						.children('a')
						.show();
	
						/**						
						* Reset collapsed widgets.
						**/
						if(thisWidgetContent.hasClass('jarviswidget-visible')){
							thisWidgetContent.hide().removeClass('jarviswidget-visible');
						}
						
					}else{
						
						/**						
						* Prevent the body from scrolling.
						**/
						$('body').addClass('nooverflow');
						
						/**						
						* Wrap, append it to the body, show the right button

						* and hide all other buttons.
						**/
						thisWidget
						.wrap('<div id="jarviswidget-fullscreen-mode"/>')
						.parent()
						.find('.jarviswidget-fullscreen-btn')
						.children()
						.removeClass(self.fullscreenClass[0])
						.addClass(self.fullscreenClass[1])
						.parents(self.pwCtrls)
						.children('a:not(.jarviswidget-fullscreen-btn)')
						.hide();
						
						/**						
						* Show collapsed widgets.
						**/
						if(thisWidgetContent.is(':hidden')){
							thisWidgetContent
							.show()
							.addClass('jarviswidget-visible');
						}
					}
					
					/**	
					* Run the set height function.
					**/	
					heightFullscreen();
	
					/**	
					* Run the callback function.
					**/	
					if(typeof self.o.onFullscreen == 'function'){
						self.o.onFullscreen.call(this, thisWidget);
					}								
	
					e.preventDefault();
				});
										
				/**	
				* Run the set fullscreen height function when the screen resizes.
				**/	
				$(window).resize(function(){
					
					/**	
					* Run the set height function.
					**/	
					heightFullscreen();
				});
	
			//*****************************************************************//
			//////////////////////////// EDIT WIDGETS ///////////////////////////
			//*****************************************************************//
						 
				/**
				* Allow users to show/hide a edit box.
				**/
				self.widget.on(clickEvent,'.jarviswidget-edit-btn', function(e){
					
					var tWidget = $(this).parents(self.o.widgets);
					
					/**						
					* Run function for the indicator image.
					**/
					self._runLoaderWidget($(this));
					
					/**						
					* Show/hide the edit box.
					**/
					if(tWidget.find(self.o.editPlaceholder).is(':visible')){
						$(this)
						.children()
						.removeClass(self.editClass[1])
						.addClass(self.editClass[0])
						.parents(self.o.widgets)
						.find(self.o.editPlaceholder)	
						.slideUp(self.o.editSpeed,function(){
							self._saveSettingsWidget();
						});	
					}else{
						$(this)
						.children()
						.removeClass(self.editClass[0])
						.addClass(self.editClass[1])
						.parents(self.o.widgets)
						.find(self.o.editPlaceholder)	
						.slideDown(self.o.editSpeed);	
					}
					
					/**	
					* Run the callback function.
					**/	
					if(typeof self.o.onEdit == 'function'){
						self.o.onEdit.call(this, tWidget);
					}								
	
					e.preventDefault();
				});
				
				/**
				* Update the widgets title by using the edit input.
				**/
				$(self.o.editPlaceholder).find('input').keyup(function(){
					$(this)
					.parents(self.o.widgets)
					.children('header')
					.children('h2')
					.text($(this).val());
				});
				
				/**
				* Set a custom style.
				**/
				self.widget.on(clickEvent,'[data-widget-setstyle]', function(e){
					
					var val    = $(this).data('widget-setstyle');
					var styles = '';
					
					/**
					* Get all other styles, in order to remove it.
					**/
					$(this).parents(self.o.editPlaceholder).find('[data-widget-setstyle]').each(function(){
						styles += $(this).data('widget-setstyle')+' ';
					});
	
					/**
					* Set the new style.
					**/
					$(this).parents(self.o.widgets).attr('data-widget-attstyle', ''+val+'').removeClass(styles).addClass(val);
					
					/**						
					* Run function for the indicator image.
					**/
					self._runLoaderWidget($(this));
					
					/**						
					* Lets save the setings.
					**/						
					self._saveSettingsWidget();	
					
					e.preventDefault();
				});	
	
			//*****************************************************************//
			/////////////////////////// CUSTOM ACTION ///////////////////////////
			//*****************************************************************//
							 
				/**
				* Allow users to show/hide a edit box.
				**/
				self.widget.on(clickEvent,'.jarviswidget-custom-btn', function(e){
	                
					var w = $(this).parents(self.o.widgets);
					
					/**						
					* Run function for the indicator image.
					**/
					self._runLoaderWidget($(this));
					
					/**						
					* Start and end custom action.
					**/
					if($(this).children('.'+self.customClass[0]).length){
						$(this)
						.children()
						.removeClass(self.customClass[0])
						.addClass(self.customClass[1]);						
						
						/**	
						* Run the callback function.
						**/	
						if(typeof self.o.customStart == 'function'){
							self.o.customStart.call(this, w);
						}
					}else{
						$(this)
						.children()
						.removeClass(self.customClass[1])
						.addClass(self.customClass[0]);
	
						/**	
						* Run the callback function.
						**/	
						if(typeof self.o.customEnd == 'function'){
							self.o.customEnd.call(this, w);
						}					
					}
						
					/**						
					* Lets save the setings.
					**/	
					self._saveSettingsWidget();							
	
					e.preventDefault();
				});
	
			//*****************************************************************//
			/////////////////////////// DELETE WIDGETS //////////////////////////
			//*****************************************************************//
							 
				/**
				* Allow users to delete the widgets.
				**/
				self.widget.on(clickEvent,'.jarviswidget-delete-btn', function(e){
					
					var tWidget  = $(this).parents(self.o.widgets);
					var removeId = tWidget.attr('id');
					var widTitle = tWidget.children('header').children('h2').text();
	
					/**
					* Delete the widgets with a confirm popup.
					**/
					var cleared = confirm(self.o.labelDelete+' "'+widTitle+'"');
					if(cleared){ 	
						
						/**						
						* Run function for the indicator image.
						**/
						self._runLoaderWidget($(this));
						
						/**						
						* Delete the right widget.
						**/
						$('#'+removeId).fadeOut(self.o.deleteSpeed, function(){
							
							$(this).remove();
							
							/**	
							* Run the callback function.
							**/	
							if(typeof self.o.onDelete == 'function'){
								self.o.onDelete.call(this, tWidget);
							}								
						});
					}
				
					e.preventDefault();
				});
			
			//******************************************************************//
			/////////////////////////// REFRESH BUTTON ///////////////////////////
			//******************************************************************//
		
				/**	
				* Refresh ajax upon clicking refresh link.
				**/
				self.widget.on(clickEvent, '.jarviswidget-refresh-btn', function(e){
					
					/**	
					* Variables.
					**/
					var rItem          = $(this).parents(self.o.widgets);
					var pathToFile     = rItem.data('widget-load');							
					var ajaxLoader     = rItem.children();	
					
					/**					
					* Run the ajax function.
					**/
					self._loadAjaxFile(rItem, pathToFile, ajaxLoader);
	
					e.preventDefault();							
				});	
        },
		
		/**
		* Destroy.
		*
		* @param:
		**/
		destroy: function(){
            var self = this;
			self.widget.off('click',self._clickEvents());
			self.obj.removeData(pluginName);		
		}
	};

	$.fn[pluginName] = function(option) {
  		return this.each(function() {
			var $this   = $(this);
            var data    = $this.data(pluginName);
            var options = typeof option == 'object' && option;
			if (!data){ 
			  $this.data(pluginName, (data = new Plugin(this, options)))
			}
			if (typeof option == 'string'){
				 data[option]();
			}
		});
	};
	
	/**
	* Default settings(dont change).
	* You can globally override these options
	* by using $.fn.pluginName.key = 'value';
	**/
	$.fn[pluginName].defaults = {
		grid: 'section',
		widgets: '.jarviswidget',
		localStorage: true,
		deleteSettingsKey: '',
		settingsKeyLabel: 'Reset settings?',
		deletePositionKey: '',
		positionKeyLabel: 'Reset position?',
		sortable: true,
		buttonsHidden: false,
		toggleButton: true,
		toggleClass: 'min-10 | plus-10',
		toggleSpeed: 200,
		onToggle: function(){},
		deleteButton: true,
		deleteClass: 'trashcan-10',
		deleteSpeed: 200,
		onDelete: function(){},
		editButton: true,
		editPlaceholder: '.jarviswidget-editbox',
		editClass: 'pencil-10 | delete-10',
		editSpeed: 200,
		onEdit: function(){},
		fullscreenButton: true,
		fullscreenClass: 'fullscreen-10 | normalscreen-10',	
		fullscreenDiff: 3,		
		onFullscreen: function(){},
		customButton: true,
		customClass: '',
		customStart: function(){},
		customEnd: function(){},
		buttonOrder: '%refresh% %delete% %custom% %edit% %fullscreen% %toggle%',
		opacity: 1.0,
		dragHandle: '> header',
		placeholderClass: 'jarviswidget-placeholder',
		indicator: true,
		indicatorTime: 600,
		ajax: true,
		loadingLabel: 'loading...',
		timestampPlaceholder:'.jarviswidget-timestamp',
		timestampFormat: 'Last update: %m%/%d%/%y% %h%:%i%:%s%',
		refreshButton: true,
		refreshButtonClass: 'refresh-10',
		labelError:'Sorry but there was a error:',
		labelUpdated: 'Last Update:',
		labelRefresh: 'Refresh',
		labelDelete: 'Delete widget:',
		afterLoad: function(){},
		rtl: false,
		onChange: function(){},
		onSave: function(){}		
	};


})(jQuery, window, document);