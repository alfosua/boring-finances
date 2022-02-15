import * as React from "react"
import { SVGProps } from "react"

// Icon from https://remixicon.com
const SvgComponent = ({ fill, ...rest }: SVGProps<SVGSVGElement>) => (
  <svg xmlns="http://www.w3.org/2000/svg" width={24} height={24} {...rest}>
    <path fill="none" d="M0 0h24v24H0z" />
    <path
      d="M6.414 16 16.556 5.858l-1.414-1.414L5 14.586V16h1.414zm.829 2H3v-4.243L14.435 2.322a1 1 0 0 1 1.414 0l2.829 2.829a1 1 0 0 1 0 1.414L7.243 18zM3 20h18v2H3v-2z"
      fill={fill}
    />
  </svg>
)

export default SvgComponent
