$(document).ready(function(){
 
    $('.button').on('click',function(){singin();})
    
});

function singin (){
   let user={
   log : $('#log').val(),
   pas : $('#pas').val()
  } 
  console.log(user);
  console.log(JSON.stringify(user));
 let  request={"method":"test"}
 console.log($);
 let request_z= 'http://195.46.123.219:22555/?request={"method":"test"}'
 console.log(request_z)
  $.ajax({
    type:'post',
    url:request_z,
    data: request,
    cach: false,
    error:(function(){alert('ошибка')}),
    success:function(data){
      alert("zzzzzzzz");

console.log(data);
}
  });}


/*
let request = new XMLHttpRequest();
request.onreadystatechange=function(){


request.open('post','http://195.46.123.219:22555/?request={"method":"test"}}');
request.send()

if(request.readyState==4 && request.status==200){
  console.log('ddddd');
}}
;}*/