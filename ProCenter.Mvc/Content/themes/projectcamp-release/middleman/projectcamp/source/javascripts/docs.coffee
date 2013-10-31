$ ->
  $("pre").addClass("prettyprint linenums")
  prettyPrint()
  showDocs = false

  $("#docs").mouseup =>
    if showDocs == false
      $(".codehint, pre").show()
      showDocs = true
    else
      $(".codehint, pre").hide()
      showDocs = false
