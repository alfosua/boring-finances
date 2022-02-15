import * as React from "react"
import { SVGProps } from "react"

// Icon from https://remixicon.com
const SvgComponent = ({ fill, ...rest }: SVGProps<SVGSVGElement>) => (
  <svg xmlns="http://www.w3.org/2000/svg" width={24} height={24} {...rest}>
    <path fill="none" d="M0 0h24v24H0z" />
    <path
      d="M20 7v14a1 1 0 0 1-1 1H5a1 1 0 0 1-1-1V7H2V5h20v2h-2zM6 7v13h12V7H6zm1-5h10v2H7V2zm4 8h2v7h-2v-7z"
      fill={fill}
    />
  </svg>
)

export default SvgComponent