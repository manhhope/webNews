$(document).ready(function () {

		$('.listproject').cycle({ 
		    fx:      'fade', 
		    timeout:  6000, 
			speed: 2000
	//		pager:  $('.listproject').next('.nav')
		});

		$('.bannerFade ul').cycle({ 
		    fx:      'fade', 
		    timeout:  7000, 
			speed: 2000,
			pager:  $('.bannerFade ul').next('.nav')
		});

		$(".news_event .listitem").next().after("<div class='nextprev'><a class='prev'></a><a class='next'></a></div>");
		$('.listitem').cycle({ 
		    fx:      'scrollHorz', 
		    timeout:  6000, 
			speed: 800,
			next: '.next',
			prev: '.prev',
			pager:  $('.listitem').next('.nav')
		});

		$(".wrap_partner").jCarouselLite({
			visible: 8,
		    auto: 2500,
		    speed: 1000
		});
	
		$(".backtop").click(function(){
			$('html, body').animate({scrollTop:0}, 500);
		});

		if($("#scroller").length >0){
			$("#scroller").simplyScroll({
				orientation: 'vertical',
	            auto: true,
	            manualMode: 'loop',
				frameRate: 20
			});
		}

	$(".articlelist .doitac li.item").each(function(){
		var _height = $(this).height();
		var _heightitem = $(this).children(".title").children("a").height();
		var _margin = (_height - _heightitem)/2;
		$(this).children(".title").children("a").css('margin-top', _margin);
	});

	$(".codong ul li, .listvideo li, .projectcontinue ul .item").each(function(){
		var _height = $(this).height();
		var _heightitem = $(this).children(".title").children("a").height();
		var _margin = (_height - _heightitem)/2;
		$(this).children(".title").children("a").css('margin-top', _margin);
	});

	$(".gallery ul li").each(function(){
		var _height = $(this).height();
		var _heightitem = $(this).children(".title").find("span").height();
		var _margin = (_height - _heightitem)/2;
		$(this).children(".title").find("span").css('margin-top', _margin);
	});
		$("a[rel=example_group]").fancybox({
		'transitionIn' : 'none',
		'transitionOut' : 'none',
		'titlePosition' : 'over',
		'titleFormat' : function(title, currentArray, currentIndex, currentOpts) {
		return '<span id="fancybox-title-over">Image ' + (currentIndex + 1) + ' / ' + currentArray.length + ' ' + title + '</span>';
		}
		});
		
		$(".w-header-menu li").hover(
			function(){
				$(this).addClass('sfHover');
				$(this).children("ul").stop(true, true).slideDown();
			},
			function(){
				$(this).removeClass('sfHover');
				$(this).children("ul").stop(true, true).fadeOut('200');
			}
		);
		divide4(".linhvuchoatdong");
		divide4(".banlanhdao");
		divide4(".doitac");
		divide4(".gallery ul");
		divide4(".listvideo");
//		slice("#scroller");

		$(".item-1.active").parents('.item').addClass('active');
		$(".item-1.active").parent().prev().addClass('active');

	$(".gallery a").click(function(){
		$.fancybox.showActivity();
		$.ajax({
			type : "GET",
			cache : false,
			url : $(this).attr('href'),
			success: function(data) {
				var arr = [];
				var _data = data.split("[|-|]");
				if(_data.length > 1){
					for(var i = 1; i<= _data.length - 1;i++){
						var $arr = _data[i].split("[-]");
						var item = {};
						item.href = $arr[0];
						item.title = $arr[1];
						arr.push(item);
					}
					function formatTitle(title, currentArray, currentIndex, currentOpts) {
						return '<div id="FLCGroup-title">' + (title && title.length ? '' + title + '<span id="position-title">' : '' ) + (currentIndex + 1) + '/' + currentArray.length + '</span></div>';
					}
					var timer;
					$.fancybox(arr,{
						titlePosition:'inside',
						overlayOpacity:'0.7',
						overlayColor:'#000',
						changeFade:'fast',
						changeSpeed:'300',
						type:'image',
						transitionIn:'fade',
						transitionOut:'fade',
						cyclic:true,
						centerOnScroll:true,
						titleFormat:formatTitle 
					});
				}
				else{
					$.fancybox.hideActivity();
				}
			}
		}); 
		return false;
	});	

	$(".listvideo li .title a").click(function(){
		var _url = $(this).attr('rel');
		var _title = $(this).text();
		$(".view_Video .videoactive").empty();
		$(".view_Video .videoactive").append(_url);
		$(".view_Video .title").empty().append(_title);
		return false;
	});
});

function divide4(id){
	for(i=0; i< $(id).children().length; i++){
		if(i%2==0){
			$(id).children().eq(i).addClass('nomargin');
		}
	}
}


function slice(id){
	if($(id).size() > 0){
		var Init = function(){
			var len = $("li",id).size();
			var visible = 4;
			if(len > visible){					
				for(var i=0;i< Math.ceil(len/visible);i++){
					$("li",id).slice(i*visible,i*visible+visible).wrapAll("<div class='wrapLi'></div>");
				}					
				$(id).cycle({
					fx:'scrollHorz',
					speed:1000,
					timeout:3000,
			//		prev:$('.prev',id),
			//		next:$('.next',id),
					onPrevNextEvent:function(){}
				});
			//	$('.prev,.next',id).fadeIn();
			}
		}
		Init();
	}
}