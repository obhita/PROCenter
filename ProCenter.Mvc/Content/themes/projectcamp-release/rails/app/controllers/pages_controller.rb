class PagesController < ApplicationController

  def show
    render template: "pages/#{params[:id] || "index"}"
  end

end
