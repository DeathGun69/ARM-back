
$('.b_add').ready(function(){
   
    gr();
    cm();
    $('.b_add').on('click',function(){add();})
});
function gr(){ 
    /*
    let request=1
    $.ajax({
        type:'post',
        url:'http://195.46.123.219:22555/?request={"method":"select","params":{"table_name":"groups"}}',
        data: request,
        cach: false,
        error:(function(){alert('ошибка')}),
        success:function(data_g){        
            console.log ('zz')          
            console.log (data_g)           
}});
*/
}
function cm(){
    let request=1
    $.ajax({
        type:'post',
        url:'http://195.46.123.219:22555/?request={"method":"select","params":{"table_name":"competence"}}',
        data: request,
        cach: false,
        error:(function(){alert('ошибка')}),
        success:function(data_g){        
            console.log ('zz')          
            console.log (data_g) 
               for(i=0;i<data_g.result.length;i++){
                $( "#s_c" ).append( ' <option value="'+data_g.result[i].id+'">'+data_g.result[i].name+'</option>' ) 
               }

}});
}


function add (){
    let request=1
    let request_z='http://195.46.123.219:22555/?request={"method":"insert","params":{"table_name":"course","rows":[{"id":'+$('#idd').val()+',"name":"'+$('#name').val()+'","groups":["'+$('#gr').val()+'"],"id_competence":'+$('#s_c').val()+',"id_teacher":'+$('#t').val()+',"hours":'+$('#h').val()+',"univer":"'+$('#un').val()+'"}]}} '
    console.log('_____')
    console.log(request_z)
    $.ajax({
        type:'post',
        url:request_z,
        data: request,
        cach: false,
        error:(function(){alert('ошибка')}),
        success:function(data){


          alert("zzzzz");   
    console.log(data);
   
            
         }});
}