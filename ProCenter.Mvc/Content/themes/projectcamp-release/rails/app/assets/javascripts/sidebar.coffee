$ ->
  $(".sidebar-helper a").click ->
    console.log ";a;a;a"
    $('html, body').animate({
      scrollTop: $("#sidebar").offset().top
    }, 500);