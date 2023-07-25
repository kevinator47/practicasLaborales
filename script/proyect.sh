#! /bin/bash
# Funcion para ejecutar el moogle
run_moogle(){
    cd ./../moogle-main
    make dev
}

# Funcion para mostrar report y slide respect
select_report(){
    FILENAME="report"
    DIRECTORY="./../informe"
}

select_slide(){
    FILENAME="slide"
    DIRECTORY="./../presentacion"
}

#Funcion para crear los pdf a partir de los .tex
create_pdf(){
    cd "${DIRECTORY}"
    pdflatex -interaction=batchmode -halt-on-error "${DIRECTORY}/${FILENAME}.tex" >/dev/null 2>&1
    pdflatex -interaction=batchmode -halt-on-error "${DIRECTORY}/${FILENAME}.tex" >/dev/null 2>&1
    echo "${FILENAME}.pdf creado con exito"
}


#Funcion para mostrar los pdf
show_pdf(){
    
    if [ -f "${DIRECTORY}/${FILENAME}.pdf" ];then
        echo "Mostrando ${FILENAME}.pdf..."
        
        viewer=""
        
        if [ $# -eq 2 ] ;then
        	viewer="$2"
        fi
        
        if [ -z "$viewer" ]; then
        	if command -v xdg-open &>/dev/null; then
        	    viewer="xdg-open"
        
     		elif command -v open &>/dev/null; then
            		viewer="open"
            	fi
        fi
        
        
        if [ -n "$viewer" ]; then
        	$viewer "${DIRECTORY}/${FILENAME}.pdf"
        
        else
            echo "Lector de PDF no disponible"
        fi
    else 
        echo "El archivo ${FILENAME}.pdf no existe"
        create_pdf
        show_pdf
    fi
}

# Funcion para eliminar archivos auxiliares
clean_files(){
    
    cd ./../informe
    rm -f "report.aux" "report.log" "report.out" "report.synctex.gz" "report.toc" "report.fls" "report.fdb_latexmk"

    cd ./../presentacion
    rm -f "slide.aux" "slide.log" "slide.out" "slide.synctex.gz" "slide.toc" "slide.fls" "slide.fdb_latexmk" "slide.vrb" "slide.nav" "slide.snm"

    echo "Archivos auxiliares borrados"
}

# Main
case "$1" in
    run)
        run_moogle ;;
    clean)
        clean_files ;;
    report)
        select_report 
        create_pdf ;;
    slide)
        select_slide 
        create_pdf ;;
    show_report)
        select_report 
        show_pdf ;;
    show_slide)
        select_slide 
        show_pdf ;;
    *)
        echo "Opción no valida. Por favor ingrese una de las siguientes opciones : run , clean , report , slide , show_report , show_slide"
esac
