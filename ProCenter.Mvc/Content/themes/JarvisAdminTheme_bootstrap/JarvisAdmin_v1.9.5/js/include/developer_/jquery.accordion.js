(function($) {
	
	/** public methods **/
	var methods = {
		/** constructor **/
		init: function(options) {
			options = $.extend({}, $.fn.ctAccordion.defaults, options);

			return this.each(function() { //uncomment this to allow multiple instances on one constructor call
				var $menu = $(this).addClass(options.myClass).data("ctAccordion", {
					options: options
				});
				
				// set the numerical path for each ul
				var ulNumID = 1;
				$("ul", $menu).each(function() {
					$(this).data("ctAccordion", {
						numID: ulNumID++
					});
				}).find("li").addClass(options.collapsedPathClass);
				
				// hide all sub positions
				$menu.find("ul").hide();
				
				// show all default expanded positions (think stateful)
				$(options.defaultExpanded, $menu).each(function() {
					
					// if i'm expanded - remove collapsed class :)
					$(this).removeClass(options.collapsedPathClass);
				}).parents("li").addClass(options.expandedPathClass).removeClass(options.collapsedPathClass);
				
				if($(options.defaultExpanded, $menu).length) {
					setOpened.apply($menu, [$(options.defaultExpanded, $menu)]);
				}
				
				// interactivity
				$("a + ul", $menu).prev("a").addClass(options.headerClass).bind(options.event, function() {
					if($(this).next("ul").is(":not(:hidden)")) {
						// hide submenu
						setCollapsed.apply($menu, [$(this)]);
					} else {
						// show submenu
						setOpened.apply($menu, [$(this), options.oneOpenAtTime]);
					}

					return false;
				});
			});

			
				
			return this;
		},
		
		/** search method: pass searched query as an argument,
		 * 	positions having it will show and highlight automagicaly
		 */
		search: function(query) {
			var $menu = $(this),
				data = $menu.data("ctAccordion"),
				options = data.options,
				regex = new RegExp(query, "i");

			
			hideUnused.apply($menu);
			
			$("li>a, >li>ul>li>ul>li", $menu).each(function() {
				
				var matches = $(this).text().match(regex);
				
				if(matches != "" && matches != null) {
					
					var $el = $(this);
					if($(this)[0].nodeName == "LI") {
						$el = $(this).closest("ul").prev("a");
					}
					
					setOpened.apply($menu, [$el, false]);
					$el.addClass(options.foundPhraseClass);
					
				} else {
					$(this).removeClass(options.foundPhraseClass);
				}
			});

			return this;
		},
		/** 
		 * open menu on selected positions
		 */
		open: function(level1Index, level2Index) {
			var $menu = $(this),
				data = $menu.data("ctAccordion"),
				options = data.options;
			
			level1Index--;
			
			return $(this).each(function() {
				if(level1Index < 0){return;}
				var $level1 = $(">li>a", $menu);
				
				if(level1Index > $level1.length) {return;}
				var $el = $level1.eq(level1Index);
				setOpened.apply($menu, [$el, options.oneOpenAtTime]);
				
				if(level2Index != undefined) {
					level2Index--;
					
					$level2 = $el.next("ul").find(">li>a");
					if(level2Index<0 || level2Index>$level2.length) {return;}
					
					var $el = $level2.eq(level2Index);
					setOpened.apply($menu, [$el, options.oneOpenAtTime]);
						
					
				}
			});
		},
		/** Get or set any option. If no value is specified, will act as a getter **/
		option: function(key, value) {
			if  (typeof key === "string" ) {
				if ( value === undefined ) {
					// behave as a "getter"
					var $container = $(this),
						data = $container.data("ctAccordion");
					
					return data.options[key];
				} else {
					// behave as a "setter"
					var $container = $(this),
						data = $container.data("ctAccordion");
							
					data.options[key] = value;
					$container.data("ctAccordion", data);
						
					return this;
				}
			}
		}
	};
	

	/**
	 * gets the "path" to the given element
	 */
	var getNumericalPath = function($el) {
		var myNumericalPath = [];

		$el.parents("ul").each(function(i, ul) {
			var data = $(ul).data("ctAccordion");
			
			var i = parseInt(data.numID);
			if(!isNaN(i)) {
				myNumericalPath.push(i);
			}
		});
		
		return myNumericalPath;
	};
	
	
	
	/**
	 * hide all currently not used "uls" - ie
	 * those that aren't directly aboce clicked element
	 * 
	 */
	var hideUnused = function($el) {
		var $menu = $(this),
			data = $menu.data("ctAccordion"),
			options = data.options;
		
		//debugger;
		if($el === undefined) {
			// just hide all
			setCollapsed.apply($menu, [$("li."+options.expandedPathClass+" > a."+options.headerClass)]);

		} else {
			var path = getNumericalPath($el);
			

			$("ul:visible", $menu).each(function(i, ul){
				var $ul = $(ul),
					data = $ul.data("ctAccordion");
				
				var myNumID = parseInt(data.numID);
				
				if($.inArray(myNumID, path) == -1) {
					setCollapsed.apply($menu, [$($ul.prev("."+options.headerClass)[0])]);
				}
			});
		}
	};
	
	
	/**
	 * hides the node 
	 */
	var setCollapsed = function($element) {
		var $menu = $(this),
			data = $menu.data("ctAccordion"),
			options = data.options;
		
		options.onClose.apply($menu, [$element]);

		$element.nextAll("ul").slideUp("fast").find("li").removeClass(options.expandedPathClass).addClass(options.collapsedPathClass);
		$element.closest("li").removeClass(options.expandedPathClass).addClass(options.collapsedPathClass);
	};
	
	/**
	 * opens node and the path if required.
	 * Optionally closes all the other nodes.
	 */
	var setOpened = function($element, doHideUnused) {
		if(doHideUnused === undefined || doHideUnused == true) {
			hideUnused.apply($(this), [$element]);
		}
		
		var $menu = $(this),
			data = $menu.data("ctAccordion"),
			options = data.options;

		options.onOpen.apply($menu, [$element]);

		
		$element.nextAll("ul").slideDown(options.speed, options.easing);
		
		// show all that is upper if needed
		openPath.apply($(this), [$element.closest("li")]);
		
	};
	
	
	/**
	 * 
	 * Show all elements above given one.
	 * In other words: show full path to the
	 * given element
	 * 
	 * @return
	 */
	var openPath = function($li) {
		var $menu = $(this),
			data = $menu.data("ctAccordion"),
			options = data.options;

		$li.addClass(options.expandedPathClass).removeClass(options.collapsedPathClass);
		var $parentUl = $li.closest("ul").slideDown("fast");
		if($parentUl.length == 0) {
			return;
		}
		
		var $parentLi = $parentUl.closest("li");
		
		if($parentLi.length > 0) {
			openPath.apply($menu, [$($parentLi[0])]);
		}
		
		return;
	};
	
	$.fn.ctAccordion = function(method) {
		if ( methods[method] ) {
			return methods[method].apply( this, Array.prototype.slice.call( arguments, 1 ));
	    } else if ( typeof method === 'object' || ! method ) {
	    	methods.init.apply( this, arguments );	
	    } else {
	    	$.error( 'Method ' +  method + ' does not exist on 3 Level Accordion!' );
	    }  
	};
	
	
	/** default values for plugin options **/
	$.fn.ctAccordion.defaults = {
		headerClass: "head",
		defaultExpanded: ".expanded",
		expandedPathClass: "open",
		collapsedPathClass: "closed",
		foundPhraseClass: "searchMatch",
		event: "click",
		myClass: "ctAccordion",
		oneOpenAtTime: true,
		easing: "linear",
		speed: 200,
		onOpen: $.noop,
		onClose: $.noop
	};
	
})(jQuery);
